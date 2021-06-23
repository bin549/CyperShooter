using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class ShootBullet : MonoBehaviour
{
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    [HideInInspector] public Vector3 impactNormal;
    public float damage = 20f;
    public float speed = 1000.0f;
    public float explosionRadius = 0f;
    [SerializeField] private BulletAudio bulletAudio;
    [SerializeField] private float force = 700f;
    protected PhotonView photonView;

    protected void Awake()
    {
        bulletAudio = GetComponent<BulletAudio>();
        photonView = GetComponent<PhotonView>();
    }
    protected virtual void DestroyBullet()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    protected virtual void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        transform.localScale = new Vector3(Random.Range(1.5f, 2), Random.Range(1.5f, 2), Random.Range(1.5f, 2));
        photonView.RPC("SpawnProjectileEffect", RpcTarget.All);
        Invoke("DestroyBullet", 10.0f);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        bulletAudio.PlayImpactSound();
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Damage(collision.gameObject.transform);
            EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
            enemyAI.SetNewDestination(transform);
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject.transform);
        }
        if (collision.gameObject.CompareTag("Car"))
        {
            Land(collision.gameObject.transform);
        }
        if (explosionRadius > 0f)
        {
            Explode();
        }
        photonView.RPC("SpawnImpactEffect", RpcTarget.All);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    protected void SpawnProjectileEffect()
    {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            muzzleParticle.transform.rotation = transform.rotation * Quaternion.Euler(180, 0, 0);
            Destroy(muzzleParticle, 1.5f);
        }
    }

    [PunRPC]
    protected void SpawnImpactEffect()
    {
        impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
        Destroy(projectileParticle, 3f);
        Destroy(impactParticle, 5f);
    }

    protected void HitTarget(Collision collision)
    {

    }

    protected void Explode()
    {
        Collider[] collidersToHurt = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in collidersToHurt)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                Damage(collider.gameObject.transform);
            }
            if (collider.gameObject.CompareTag("Obstacle"))
            {
                Destroy(collider.gameObject.transform);
            }
            if (collider.gameObject.CompareTag("Car"))
            {
                Land(collider.gameObject.transform);
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
    }

    protected void Damage(Transform enemy)
    {
        EnemyHealth e = enemy.gameObject.GetComponent<EnemyHealth>();
        if (e != null)
        {
            e.TakeDamage(damage);
        }
        /*
        EnemyAI ea = enemy.gameObject.GetComponent<EnemyHealth>();
        if (ea != null)
{
ea.TakeDamage(damage);
}
*/
    }

    protected void Destroy(Transform obstacle)
    {
        Barrel barrel = obstacle.gameObject.GetComponent<Barrel>();
        if (barrel != null)
        {
            barrel.TakeDamage();
        }
        Destructible destructible = obstacle.gameObject.GetComponent<Destructible>();
        if (destructible != null)
        {
            destructible.TakeDamage();
        }
    }

    protected void Land(Transform car)
    {
        Car c = car.gameObject.GetComponent<Car>();
        if (c != null)
        {
            c.TakeDamage();
        }
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
