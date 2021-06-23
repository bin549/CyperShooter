using UnityEngine;

[RequireComponent(typeof(ObstacleAudio))]
public class CarExplosion : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float force = 700f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private ObstacleAudio obstacleAudio;

    private void Awake()
    {
        obstacleAudio = GetComponent<ObstacleAudio>();
    }

    private void Start()
    {
        Explode();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    protected void Explode()
    {
        GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        obstacleAudio.PlayDestorySound();

        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in collidersToDestroy)
        {
            Destructible dest = nearbyObject.GetComponent<Destructible>();
            if (dest != null)
            {
                dest.Destory();
            }
        }
        Collider[] collidersToHurt = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in collidersToHurt)
        {
            EnemyHealth enemyHealth = nearbyObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, explosionRadius);
            }
        }
        GameObject.Destroy(gameObject);
    }
}
