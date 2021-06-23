using UnityEngine;

public class Mine : Barrel
{
    public bool isTrigger;
    private float countdown;
    public float delay = 3f;
    [SerializeField] private GameObject triggerEffect;

    private void Start()
    {
        countdown = delay;
        triggerEffect.SetActive(false);
    }

    public override void TakeDamage()
    {

    }

    private void Update()
    {
        if (isTrigger)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                Explode();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            triggerEffect.SetActive(true);
            isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            countdown = delay;
            triggerEffect.SetActive(false);
            isTrigger = false;
        }
    }
}
