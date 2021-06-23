using UnityEngine;

public class Melee : MonoBehaviour
{
    public float damage = 20f;

    [SerializeField] public CustomGrabbable grabbable;
    [Range(0, 300.0f)] [SerializeField] protected float force = 300f;

    private void Awake()
    {
        grabbable = GetComponent<CustomGrabbable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Actor") || collision.transform.root.gameObject.CompareTag("Actor"))
        {
            var targetScript = collision.transform.gameObject.GetComponent<Health>() ?? collision.transform.root.gameObject.GetComponent<Health>();

            if (targetScript == null || targetScript.isPlayer) return;
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(gameObject.transform.forward * force);
            }

            targetScript.TakeDamage(damage, null);
            VibrationManager.Instance.VibrateController(0.1f, 0.4f, 1.5f, grabbable.controller);
        }
    }
}
