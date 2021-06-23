using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class Sword : MeleeWeapon
{
    [SerializeField] private float explosionRadius = 5.0f;

    protected override void Start()
    {
        base.Start();
        onEffectTrigger += Explosion;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        onEffectTrigger(collision);
    }

    private void Explosion(Collision collision)
    {
        if (UnityEngine.Random.Range(0.0f, 100.0f) < effectTriggerPossibility)
        {
          //  photonView.RPC("SpawnExplosionEffect", RpcTarget.All, collision);

            Collider[] collidersToDestroy = Physics.OverlapSphere(collision.transform.position, explosionRadius);
            foreach (Collider nearbyObject in collidersToDestroy)
            {
                Destructible dest = nearbyObject.GetComponent<Destructible>();
                if (dest != null)
                {
                    dest.Destory();
                }
            }
            Collider[] collidersToHurt = Physics.OverlapSphere(collision.transform.position, explosionRadius);
            foreach (Collider nearbyObject in collidersToHurt)
            {
                EnemyHealth enemyHealth = nearbyObject.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }
            Collider[] collidersToMove = Physics.OverlapSphere(collision.transform.position, explosionRadius);
            foreach (Collider nearbyObject in collidersToMove)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null && !rb.gameObject.CompareTag("Player"))
                {
                    rb.AddExplosionForce(force, collision.transform.position, explosionRadius);
                }
            }
        }
    }

    [PunRPC]
    private void SpawnExplosionEffect(Collision collision)
    {
        weaponAudio.PlayShootSound();
        GameObject.Instantiate(effect, collision.transform.position, collision.transform.rotation);
    }
}
