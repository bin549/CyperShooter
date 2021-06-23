using UnityEngine;
using UnityEngine.Events;

public class MeleeWeapon : UpgradeWeaponController
{
    public float damage = 20f;
    [SerializeField] protected float force = 100f;
    [SerializeField] protected GameObject effect;
    [SerializeField] protected GameObject[] effects;
    [Range(0, 100.0f)] [SerializeField] protected float effectTriggerPossibility;
    private int nextEffectIndex = 0;
    public float upgradeeffectTriggerPossibility = 0.2f;
    public UnityAction<Collision> onEffectTrigger;

    protected virtual void Start()
    {

    }

    protected override void Update()
    {
        base.Update();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(-collision.gameObject.transform.forward * force);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            VibrationManager.Instance.VibrateController(0.1f, 0.4f, 1.5f, controller);
        }
    }

    public override void UpgradeWeapon()
    {
        base.UpgradeWeapon();
        if (effects[nextEffectIndex] != null)
        {
            effect = effects[nextEffectIndex];
            effectTriggerPossibility += upgradeeffectTriggerPossibility;
        }
        nextEffectIndex++;
    }
}
