using UnityEngine;

public class BombableWeapon : ThrowWeapon
{
    [SerializeField] protected GameObject[] explosionEffects;
    [SerializeField] protected GameObject explosionEffect;
    [SerializeField] protected float explosionRadius = 5f;
    protected float damage = 20f;
    protected float force = 700f;
    [SerializeField] protected bool isReady = false;
    protected bool hasExploded = false;
    protected float upgradeScale = 0.2f;
    protected float upgradeDamage = 20f;

    protected void Explode()
    {
        GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in collidersToDestroy)
        {
            Destructible dest = nearbyObject.GetComponent<Destructible>();
            if (dest != null)
            {
                dest.Destory();
            }
        }
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null && !rb.gameObject.CompareTag("Player"))
            {
                rb.AddExplosionForce(force, transform.position, explosionRadius);
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
    }

    protected void Damage(Transform enemy)
    {
        EnemyHealth e = enemy.gameObject.GetComponent<EnemyHealth>();

        if (e != null)
        {
            e.TakeDamage(damage);
        }
    }

    public override void UpgradeWeapon()
    {
        damage += upgradeDamage;
        explosionEffect.transform.localScale = new Vector3(explosionEffect.transform.localScale.x + upgradeScale, explosionEffect.transform.localScale.y + upgradeScale, explosionEffect.transform.localScale.z + upgradeScale);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
