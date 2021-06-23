using UnityEngine;
using Photon.Pun;

public class Barrel : Obstacle
{
    [SerializeField] protected GameObject explosionEffect;
    [SerializeField] protected float damage = 20f;
    [SerializeField] protected float force = 700f;
    [SerializeField] protected float explosionRadius = 5f;

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public override void TakeDamage()
    {
        Explode();
    }

    protected void Explode()
    {
        photonView.RPC("SpawnExplosionEffect", RpcTarget.All);

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
            if (rb != null && !rb.gameObject.CompareTag("Player"))
            {
                rb.AddExplosionForce(force, transform.position, explosionRadius);
            }
        }
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    protected void SpawnExplosionEffect()
    {
        obstacleAudio.PlayDestorySound();
        var explosion = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosion, 3.0f);
    }
}
