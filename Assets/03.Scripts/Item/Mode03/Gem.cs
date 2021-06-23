using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(AudioSource))]
public class Gem : MonoBehaviour
{
    public float perceptionRange;
    public float triggerRange;
    public PhotonView photonView;
    public float speed = 100f;
    public int EXP_POINT = 5;
    [SerializeField] private Transform target;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collSound;
    [SerializeField] private GameObject gemImpact;
    public float randomPercent = 10;
    public bool isReach = false;
    public float radiusMin;
    public float radiusMax;
    public Transform reachPoint;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        audioSource.pitch *= 1 + Random.Range(-randomPercent / 100, randomPercent / 100);
        SetReachPoint();
    }

    private void Update()
    {
        if (!isReach)
        {
            transform.position = Vector3.Lerp(transform.position, reachPoint.position, 0.02f * Time.deltaTime);
            if (Vector3.Distance(transform.position, reachPoint.position) < 0.001f)
            {
                isReach = true;
            }
        }
        if (target == null)
        {
            Perception();
        }
        else
        {
            Trigger();
        }
    }

    private void SetReachPoint()
    {
        float rand_Radius = Random.Range(radiusMin, radiusMax);
        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;
        reachPoint.position = new Vector3(randDir.x, randDir.y, randDir.z);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        //AudioManager.Instance.Play(audioSource, collSound);
    }

    private void Perception()
    {
        UpgradeWeaponController[] weapons = FindObjectsOfType<UpgradeWeaponController>();
        float shortestDistance = Mathf.Infinity;
        UpgradeWeaponController nearestWeapon = null;
        foreach (UpgradeWeaponController weapon in weapons)
        {
            float distanceToWeapon = Vector3.Distance(transform.position, weapon.transform.position);
            if (distanceToWeapon < shortestDistance)
            {
                shortestDistance = distanceToWeapon;
                nearestWeapon = weapon;
            }
        }
        if (nearestWeapon != null && shortestDistance <= perceptionRange)
        {
            target = nearestWeapon.transform;
            //  GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void Trigger()
    {
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
        if (Vector3.Distance(transform.position, target.position) < triggerRange)
        {
            target.GetComponent<UpgradeWeaponController>().AddEXP(EXP_POINT);
            photonView.RPC("SpawnTriggerEffect", RpcTarget.All);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    private void SpawnTriggerEffect()
    {
        GameObject impact = Instantiate(gemImpact, transform.position, Quaternion.identity);
        Destroy(impact, 1.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, perceptionRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
}
