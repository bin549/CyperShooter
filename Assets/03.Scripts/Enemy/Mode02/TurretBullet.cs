using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TurretBullet : MonoBehaviour
{
    [SerializeField] private Transform target;

    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;

    [HideInInspector] public Vector3 impactNormal;
    public float damage = 20f;
    public float speed = 1000.0f;
    public float explosionRadius = 0f;
    [SerializeField] private BulletAudio bulletAudio;

    protected void Awake()
    {
        bulletAudio = GetComponent<BulletAudio>();
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            muzzleParticle.transform.rotation = transform.rotation * Quaternion.Euler(180, 0, 0);
            Destroy(muzzleParticle, 1.5f);
        }
    }

    protected virtual void Start()
    {
        Destroy(gameObject, 10.5f);
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        transform.localScale = new Vector3(Random.Range(1.5f, 2), Random.Range(1.5f, 2), Random.Range(1.5f, 2));
    }

    public void Seek(Transform target)
    {
        this.target = target;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponentInParent<Health>())
        {
            collision.transform.GetComponentInParent<Health>().TakeDamage(damage, null);
        }
    }

    private void Update()
    {
        /*
          Vector3 dir = target.position - transform.position;
          float distanceThisFrame = speed * Time.deltaTime;
          transform.Translate(dir.normalized * distanceThisFrame, Space.World);
          transform.LookAt(target);
          */
    }
}
