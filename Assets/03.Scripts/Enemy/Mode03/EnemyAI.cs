using UnityEngine;
using Photon.Pun;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    public GameObject impactEffect;

    [Header("Base")]
    public float startSpeed = 10f;
    public float speed = 70f;
    public int damage = 50;
    public Transform startPoint;
    public Transform hitCheckPoint;
    public const float SPEEDPLUS = 1.4f;

    [SerializeField] private float force = 7f;
    [SerializeField] private float strongAttackForce = 7f;
    public float strongAttackRange;
    public bool haveStrongAttackRange = false;
    [Range(0, 1.0f)] [SerializeField] private float strongAttackPossibility = 0.7f;


    [Header("Status")]
    public bool isPatrolling = true;

    [Header("Radius")]
    public float activityRadius;
    public float patrolRadius_Min;
    public float patrolRadius;
    public float explosionRange;
    public float attackRange;

    protected float elapsedTime;
    public float attackRate = 2.4f;

    private EnemyAnimator enemyAnimator;

    private float patrol_Timer;
    public float patrol_For_This_Time = 15f;

    private EnemyAudio enemyAudio;

    private void Awake()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyAudio = GetComponent<EnemyAudio>();
    }

    private void Start()
    {
        startPoint.SetParent(transform.parent);
        speed = startSpeed;
        patrol_For_This_Time = Random.Range(8f, 15f);
    }

    private void Update()
    {
        if (target == null)
        {
            Patrol();
        }
        else
        {
            ChaseTarget();
        }
    }

    private void Patrol()
    {
        AvoidStupid();
        if (!FindPlayer() && !isPatrolling)
        {
            enemyAnimator.Run(false);
            enemyAnimator.Walk(true);
            transform.position = Vector3.Lerp(transform.position, startPoint.position, speed * Time.deltaTime);
            transform.LookAt(startPoint);
            //  enemyAudio.PlayWalkSound();
            if (Vector3.Distance(transform.position, startPoint.position) < 1.4f)
            {
                enemyAnimator.Walk(false);
                isPatrolling = true;
            }
        }
        else
        {
            patrol_Timer += Time.deltaTime;
            if (patrol_Timer > patrol_For_This_Time)
            {
                patrol_Timer = 0f;
                SetNewRandomDestination();
            }
        }
    }

    private void AvoidStupid()
    {
        Ray ray = new Ray();
        ray.direction = hitCheckPoint.forward;
        ray.origin = hitCheckPoint.position;
        bool hitCheck = Physics.Raycast(hitCheckPoint.position, hitCheckPoint.forward, out RaycastHit hit, 0.1f);
        Debug.DrawRay(hitCheckPoint.position, hitCheckPoint.forward, Color.red, 0.1f);
        if (hitCheck)
        {
            if (!hit.collider.gameObject.CompareTag("Player"))
            {
                SetNewRandomDestination();
            }
        }
    }

    private void SetNewRandomDestination()
    {
        float rand_Radius = Random.Range(patrolRadius_Min, patrolRadius);
        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;
        startPoint.position = new Vector3(randDir.x, -4f, randDir.z);
        isPatrolling = false;
    }

    public void SetNewDestination(Transform transform)
    {
        if (isPatrolling)
        {
            startPoint.position = -transform.forward * patrolRadius;
            isPatrolling = false;
        }
    }

    public void ChaseTarget()
    {
        if (Vector3.Distance(startPoint.position, transform.position) >= activityRadius)
        {
            target = null;
            return;
        }
        enemyAnimator.Run(true);
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime * SPEEDPLUS;
        //    enemyAudio.PlayRunSound();
        if (Vector3.Distance(transform.position, target.position) < attackRange)
        {
            enemyAnimator.Run(false);
            enemyAnimator.Walk(false);
            AttackTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    private void AttackTarget()
    {
        if (explosionRange > 0f)
        {
            Explode();
        }
        else
        {
            elapsedTime += Time.deltaTime;
            bool useStrongAttack = UnityEngine.Random.Range(0.0f, 1.0f) > strongAttackPossibility ? true : false;
            if (elapsedTime > attackRate)
            {
                if (!useStrongAttack)
                {
                    enemyAudio.PlayAttackSound();
                    enemyAnimator.Attack();
                    Damage(target);
                }
                else
                {

                    enemyAudio.PlayStrongAttackSound();
                    enemyAnimator.StrongAttack();

                    if (haveStrongAttackRange)
                    {
                        RangeDamage();
                    }
                    else
                    {
                        Rigidbody rb = target.gameObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.AddForce(-target.forward * strongAttackForce);
                        }
                        Damage(target);
                    }

                }

                elapsedTime = 0f;
            }
        }
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    public bool FindPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.PLAYER);
        float shortestDistance = Mathf.Infinity;
        float distanceToShortest = Mathf.Infinity;
        GameObject nearestPlayer = null;
        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = player;
                distanceToShortest = Vector3.Distance(startPoint.position, player.transform.position);
            }
        }
        if (nearestPlayer != null && shortestDistance <= patrolRadius && distanceToShortest <= activityRadius)
        {
            target = nearestPlayer.transform;
            isPatrolling = false;
        }
        return target != null ? true : false;
    }

    private void Explode()
    {
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, explosionRange);
            }
        }
        Collider[] collidersToHurt = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (Collider nearbyObject in collidersToHurt)
        {
            Damage(nearbyObject.gameObject.transform);
        }
        PhotonNetwork.Destroy(this.gameObject);
    }

    private void RangeDamage()
    {
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, strongAttackRange);
        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(strongAttackForce, transform.position, strongAttackRange);
            }
        }
        Collider[] collidersToHurt = Physics.OverlapSphere(transform.position, strongAttackRange);
        foreach (Collider nearbyObject in collidersToHurt)
        {
            Damage(nearbyObject.gameObject.transform);
        }
    }
    /*
        public void target(Transform target)
    {
    this.target = target;
    }*/

    private void Damage(Transform target)
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activityRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, strongAttackRange);

    }
}
