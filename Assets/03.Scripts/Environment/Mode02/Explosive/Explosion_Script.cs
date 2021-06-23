using System.Collections;
using UnityEngine;

public abstract class Explosion_Script : MonoBehaviour
{
    private Health health;
    public ConfigurationScript m_ConfigurationScript;
    public float ExplodeDamage;
    private float randomTime;

    protected virtual void Start()
    {
        health = GetComponent<Health>();
        health.onDie += OnDie;
        randomTime = Random.Range(m_ConfigurationScript.minTime, m_ConfigurationScript.maxTime);
    }

    public virtual void OnDie()
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(randomTime);

        Vector3 explosionPos = transform.position;

        RaycastHit checkGround;
        if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
        {
            Instantiate(m_ConfigurationScript.ExplodePrefab, checkGround.point,
                Quaternion.FromToRotation(Vector3.forward, checkGround.normal));
        }

        Collider[] colliders = Physics.OverlapSphere(explosionPos, m_ConfigurationScript.explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(m_ConfigurationScript.explosionForce * 50, explosionPos, m_ConfigurationScript.explosionRadius);

            Health HitHealth = hit.GetComponentInParent<Health>();
            if (HitHealth)
            {
                HitHealth.TakeDamage(ExplodeDamage, null);
            }
            Destroy(gameObject);
        }
    }
}
