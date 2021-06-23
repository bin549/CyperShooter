using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WaspQueen : MonoBehaviour
{
    [SerializeField] private Wasp waspPrefab;
    private Transform SpawnTransform;
    private Transform target;

    [Header("Effect")]
    [SerializeField] private GameObject deadEffect;
    [SerializeField] private GameObject spawnEffect;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip spawnAudio, hurtSound, dieSound;

    public int moveSpeed, rotationSpeed;
    public int maxDistance;

    private Transform myTransform;
    public float seePlayerRadius;

    private void Awake()
    {
        myTransform = transform;
    }

    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        maxDistance = 2;
    }

    private void Update()
    {
        Debug.DrawLine(target.position, myTransform.position, Color.yellow);
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
        if (Vector3.Distance(target.position, myTransform.position) > maxDistance)
        {
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        }
        SpawnMob();
    }

    private void SpawnMob()
    {
        Wasp go = Instantiate(waspPrefab, SpawnTransform.position, Quaternion.identity) as Wasp;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, seePlayerRadius);
    }
}
