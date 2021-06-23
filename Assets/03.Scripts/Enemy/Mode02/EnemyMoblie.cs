using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoblie : MonoBehaviour
{
    public enum AIState
    {
        Idle,
        Attack,
        Hide,
    }

    public AIState aiState { get; private set; }

    [Header("Move Range")]
    public float fieldOfViewAngle = 110f;
    public float seePlayerRadius;
    public float attackRadius;
    public float stopRunRadius;
    public float bettwenHideTime;

    private float SqrSeePlayerRadius;
    private float SqrAttackRadius;
    private float damageValue;
    private float lastHideTime;

    private bool noticedBeAttacked;
    private bool dirveRollIsPlay;

    public Transform detectionSourcePoint;
    public Transform aimingTarget;
    public Transform weaponHolderTransform;

    public GameObject knownDetectedTarget { get; private set; }

    public GameObject dieBloodPrefab;
    public GameObject diedBodyPrefab;

    private ActorsManager actorsManager;
    private Actor actor;
    private Health health;
    private NavMeshAgent navMeshAgent;
    private Vector3 originAimingTarget;
    private Animator animator;
    private IWeapon iweapon;
    private Projector projector;
    private AnimatorStateInfo gunStateInfo;

    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        projector = gameObject.GetComponentInChildren<Projector>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        actorsManager = FindObjectOfType<ActorsManager>();
        actor = GetComponent<Actor>();
        health = gameObject.GetComponent<Health>();
        iweapon = weaponHolderTransform.GetComponent<IWeapon>();
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
        if (gameManager.gameIsOver)
            return;
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
                AnimatorSetup();
                break;
            case AIState.Hide:
                //navMeshAgent.destination = null;
                //animator.Play("DiveRoll", 1, 0.0f);
                //aiState = AIState.Attack;
                if (!dirveRollIsPlay)
                {
                    string DiveRoll = "DiveRoll";
                    animator.Play(DiveRoll, 0, 0.0f);
                    StartCoroutine(DiveRollAnimation(DiveRoll));
                }
                break;
            default:
                break;
        }
    }

    private IEnumerator DiveRollAnimation(string CheckName)
    {
        dirveRollIsPlay = true;

        while (true)
        {
            yield return null;
            gunStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (gunStateInfo.IsTag("DiveRoll"))
            {
                if (gunStateInfo.normalizedTime >= 0.90f)
                {
                    aiState = AIState.Attack;
                    dirveRollIsPlay = !dirveRollIsPlay;
                    yield break;
                }
            }
        }
    }

    private void AnimatorSetup()
    {
        float speed = Vector3.Dot(navMeshAgent.desiredVelocity, transform.forward);
        float angle = FindAngle(transform.forward, navMeshAgent.desiredVelocity, transform.up);
        animator.SetFloat("Vertical", speed, 0, Time.deltaTime);
        animator.SetFloat("Horizontal", angle, 0, Time.deltaTime);
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
        damageValue += damage;

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
        /*
        if (damageValue >= 60f && IsAllowHiding())
          {
              aiState = AIState.Hide;
              damageValue = 0f;
              lastHideTime = Time.time;
          }
          */
    }

    private bool IsAllowHiding()
    {
        return Time.time - lastHideTime > bettwenHideTime;
    }
    //Vector3 tmpgameObjectVector3 = transform.rotation.eulerAngles;
    //Vector3 BloodAngle = new Vector3(-90, 0, tmpgameObjectVector3.y);
    //Instantiate(dieBloodPrefab, hit.point, Quaternion.Euler(BloodAngle));

    private void OnDie()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Instantiate(dieBloodPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        }
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

    public void HandleAttackTarget(GameObject TargetGameObject)
    {
        aimingTarget.position = Vector3.Lerp(aimingTarget.position, TargetGameObject.transform.position, Time.deltaTime * 6f);
        detectionSourcePoint.LookAt(new Vector3(aimingTarget.position.x, detectionSourcePoint.position.y, aimingTarget.position.z));
        Quaternion lookatrotation = detectionSourcePoint.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, lookatrotation, Time.deltaTime * 4f);
        navMeshAgent.destination = aimingTarget.position;

        if (aimingTarget.localPosition.sqrMagnitude < SqrAttackRadius)
        {
            RaycastHit hit;
            if (Physics.Raycast(detectionSourcePoint.position, (aimingTarget.position - detectionSourcePoint.position).normalized, out hit, attackRadius))
            {
                if (hit.collider.GetComponentInParent<Actor>())
                {
                    iweapon.DoAttack();
                }
            }
        }
    }
}
