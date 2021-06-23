using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HoverBot : MonoBehaviour
{
    public AIState aiState { get; private set; }

    public enum AIState
    {
        Idle,
        Attack,
    }

    [Header("Move Range")]
    public float fieldOfViewAngle = 110f;
    public float seePlayerRadius;
    public float attackRadius;
    public float stopRunRadius;

    private float SqrSeePlayerRadius;
    private float SqrAttackRadius;
    private float damageValue;

    private bool noticedBeAttacked;

    public Transform detectionSourcePoint;
    public Transform aimingTarget;

    public GameObject knownDetectedTarget;

    public GameObject diedBodyPrefab;

    private ActorsManager actorsManager;
    private Actor actor;
    private Health health;
    private NavMeshAgent navMeshAgent;
    private Vector3 originAimingTarget;
    private Animator animator;
    private Projector projector;

    private void Awake()
    {
        projector = gameObject.GetComponentInChildren<Projector>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        actorsManager = FindObjectOfType<ActorsManager>();
        actor = GetComponent<Actor>();
        health = gameObject.GetComponent<Health>();
    }

    private void Start()
    {
        originAimingTarget = aimingTarget.localPosition;
        aiState = AIState.Idle;
        SqrSeePlayerRadius = seePlayerRadius * seePlayerRadius;
        SqrAttackRadius = attackRadius * attackRadius;
        health.onDie += OnDie;
        health.onDamaged += OnDamaged;
        navMeshAgent.updateRotation = false;
        navMeshAgent.stoppingDistance = stopRunRadius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, seePlayerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopRunRadius);
    }

    private void Update()
    {
        switch (aiState)
        {
            case AIState.Idle:
                if (originAimingTarget != null)
                    aimingTarget.localPosition = originAimingTarget;
                animator.SetBool("Fighting", false);
                HandleTargetDetection(actor);
                break;
            case AIState.Attack:
                if (!knownDetectedTarget)
                {
                    aiState = AIState.Idle;
                    return;
                }
                noticedBeAttacked = false;
                HandleAttackTarget(knownDetectedTarget);
                break;
            default:
                break;
        }
    }

    private float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        if (toVector == Vector3.zero)
            return 0f;
        float angle = Vector3.Angle(fromVector, toVector);
        Vector3 normal = Vector3.Cross(fromVector, toVector);
        angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
        angle *= Mathf.Deg2Rad;
        return angle;
    }

    private void OnDamaged(float damage, GameObject damageSource)
    {
        if (this.aiState == AIState.Idle)
        {
            //noticedBeAttacked = true;
            if (damageSource)
            {
                knownDetectedTarget = damageSource;
                aiState = AIState.Attack;
            }
            else
            {
                noticedBeAttacked = true;
            }
            return;
        }
    }

    private void OnDie()
    {
        Instantiate(diedBodyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void HandleTargetDetection(Actor actor)
    {
        foreach (Actor otherActor in actorsManager.actors)
        {
            if (otherActor.affiliation != actor.affiliation)
            {
                if ((otherActor.aimPoint.position - detectionSourcePoint.position).sqrMagnitude < SqrSeePlayerRadius)
                {
                    if ((otherActor.aimPoint.position - detectionSourcePoint.position).sqrMagnitude < SqrSeePlayerRadius)
                    {
                        Vector3 direction = otherActor.aimPoint.position - detectionSourcePoint.position;
                        float angle = Vector3.Angle(direction, transform.forward);
                        if (angle < fieldOfViewAngle * 0.5f || noticedBeAttacked)
                        {
                            RaycastHit hit;
                            if (Physics.Raycast(detectionSourcePoint.position, direction.normalized, out hit, seePlayerRadius))
                            {
                                Actor hitActor = hit.collider.GetComponentInParent<Actor>();
                                if (hitActor == otherActor)
                                {
                                    if (projector)
                                        projector.enabled = false;
                                    aiState = AIState.Attack;
                                    animator.SetBool("Fighting", true);
                                    knownDetectedTarget = otherActor.aimPoint.gameObject;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void HandleAttackTarget(GameObject TargetGameObject)
    {
        if (Vector3.Distance(transform.position, knownDetectedTarget.transform.position) < SqrAttackRadius)
        {
            // Pause
            animator.Play("Shoot", 1, 0.0f);
        }
        else if (Vector3.Distance(transform.position, knownDetectedTarget.transform.position) > stopRunRadius)
        {
            knownDetectedTarget = null;
        }
        else
        {
            aimingTarget.position = Vector3.Lerp(aimingTarget.position, TargetGameObject.transform.position, Time.deltaTime / 2);
            detectionSourcePoint.LookAt(new Vector3(aimingTarget.position.x, detectionSourcePoint.position.y, aimingTarget.position.z));
            Quaternion lookatrotation = detectionSourcePoint.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, lookatrotation, Time.deltaTime);
            navMeshAgent.destination = aimingTarget.position;

        }
    }
}
