using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Actor targetActor;
    [Header("General")]
    public float range = 15f;

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string actorTag = "Actor";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Transform firePoint;

    private Animator animator;
    private Health health;
    public GameObject diedBodyPrefab;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        health = gameObject.GetComponent<Health>();
    }

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        health.onDie += OnDie;
        health.onDamaged += OnDamaged;
    }

    private void OnDie()
    {
        Instantiate(diedBodyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDamaged(float damage, GameObject damageSource)
    {
        animator.SetTrigger("OnDamaged");
    }

    private void UpdateTarget()
    {
        GameObject[] actors = GameObject.FindGameObjectsWithTag(actorTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestActor = null;
        foreach (GameObject actor in actors)
        {
            float distanceToActor = Vector3.Distance(transform.position, actor.transform.position);
            if (distanceToActor < shortestDistance)
            {
                shortestDistance = distanceToActor;
                nearestActor = actor;
            }
        }
        if (nearestActor != null && shortestDistance <= range)
        {
            animator.SetBool("IsActive", true);
            target = nearestActor.transform;
            targetActor = nearestActor.GetComponent<Actor>();
        }
        else
        {
            animator.SetBool("IsActive", false);
            target = null;
        }
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }
        LockOnTarget();
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void LockOnTarget()
    {
        /*Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y+90.0f, 0f);*/
    }

    private void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        TurretBullet bullet = bulletGO.GetComponent<TurretBullet>();
        if (bullet != null)
            bullet.Seek(target);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
