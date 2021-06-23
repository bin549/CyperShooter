using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityAction<float, GameObject> onDamaged;
    public UnityAction onDie;
    public float currentHealth;
    public float maxHealth = 100f;
    public bool isInvincible;
    private bool isDead;
    public AudioSource audioSource;
    [SerializeField] public bool isPlayer;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        if (isPlayer)
        {
            onDamaged += PlayerDamage;
            onDie += PlayerDie;
        }
    }

    private void PlayerDamage(float damage, GameObject damageSource)
    {
        GetComponent<PlayerConstraint>().gameCanvas.Hurt();
        if (currentHealth < 20)
        {
            GetComponent<PlayerConstraint>().gameCanvas.Dangerous();
        }
    }

    private void PlayerDie()
    {
        gameManager.LoseLevel();
    }

    public void TakeDamage(float damage, GameObject damageSource)
    {
        if (isInvincible)
            return;
        float healthBefore = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        float trueDamageAmount = healthBefore - currentHealth;
        Debug.Log(trueDamageAmount);
        if (trueDamageAmount > 0f && onDamaged != null)
        {
            onDamaged.Invoke(trueDamageAmount, damageSource);
        }
        HandleDeath();
    }

    private void HandleDeath()
    {
        if (isDead)
            return;
        if (currentHealth <= 0f)
        {
            if (onDie != null)
            {
                isDead = true;
                onDie.Invoke();
            }
        }
    }

    public void HitAction(Vector3 tr)
    {

    }
}
