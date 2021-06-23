using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Wasp : MonoBehaviour
{
    public Transform target;
    public Animator animator;
    public int moveSpeed, rotationSpeed;
    public float attackTimer;
    public float coolDown;
    public float maxDistance;
    public float damage;

    [Header("Effect")]
    [SerializeField] private GameObject deadEffect;
    [SerializeField] private GameObject spawnEffect;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip spawnAudio, hurtSound, dieSound;

    private void Start()
    {
        attackTimer = 0;
        coolDown = 2.0f;
    }

    private void Update()
    {
        if (target == null)
        {
            DefineTarget();
        }
        else
        {
            ChaseTarget();
        }
    }

    private void DefineTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.PLAYER);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;
        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = player;
            }
        }
        if (nearestPlayer != null)
        {
            target = nearestPlayer.transform;
        }
    }

    private void ChaseTarget()
    {
        Debug.DrawLine(target.position, transform.position, Color.yellow);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotationSpeed * Time.deltaTime);
        if (Vector3.Distance(target.position, transform.position) > maxDistance)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
            attackTimer = 0;

        if (attackTimer == 0)
        {
            Attack();
        }
    }

    private void Attack()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        Vector3 dir = (target.transform.position - transform.position).normalized;
        float direction = Vector3.Dot(dir, transform.forward);

        if (distance <= 2.5f && direction > 0)
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                animator.SetTrigger("attack");
                attackTimer = coolDown;
            }
        }
    }
}
