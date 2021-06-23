using UnityEngine;

[RequireComponent(typeof(ThrowGrabbable))]
public class Throw : MonoBehaviour
{
    private ThrowGrabbable throwGrabbable;
    public float delay = 3f;
    private float countdown;
    private bool hasExploded;
    [SerializeField] protected GameObject explosionEffect;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float force = 700f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private AudioSource audioSource;
    public FootstepAudioData FootstepAudioData;
    [SerializeField] private AudioClip explodeSound;

    private void Start()
    {
        throwGrabbable = GetComponent<ThrowGrabbable>();
        countdown = delay;
    }

    private void Update()
    {
        if (throwGrabbable.canExplose && !hasExploded)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                audioSource.clip = explodeSound;
                audioSource.Play();
                hasExploded = true;
                Explode();
            }
        }
    }

    private void Explode()
    {
        GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = null;
            if (nearbyObject.transform.root.gameObject.CompareTag("Actor"))
            {
                rb = nearbyObject.transform.root.GetComponent<Rigidbody>();
            }
            else
            {
                rb = nearbyObject.GetComponent<Rigidbody>();
            }
            if (rb != null && !rb.gameObject.CompareTag("Player"))
            {
                rb.AddExplosionForce(force, transform.position, explosionRadius);
            }
        }
        Collider[] collidersToHurt = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in collidersToHurt)
        {
            Health health = nearbyObject.GetComponent<Health>() ?? nearbyObject.transform.root.gameObject.GetComponent<Health>(); ;
            if (health != null)
            {
                health.TakeDamage(damage, null);
            }
        }
        GameObject.Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var tmp_AudioElement in FootstepAudioData.FootstepAudios)
        {
            if (collision.gameObject.CompareTag(tmp_AudioElement.Tag))
            {
                int tmp_AudioCount = tmp_AudioElement.AduioClips.Count;
                int tmp_AudioIndex = UnityEngine.Random.Range(0, tmp_AudioCount);
                AudioClip tmp_FootsetpAudioClip = tmp_AudioElement.AduioClips[tmp_AudioIndex];
                audioSource.clip = tmp_FootsetpAudioClip;
                audioSource.Play();
            }
        }
    }
}
