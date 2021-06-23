using UnityEngine;
using Photon.Pun;

public class EnemyHealth : CooperationModeHealth
{
    [SerializeField] private EnemyDropout enemyDropout;
    private Animator animator;
    public WaveSpawner waveSpawner;
    public ItemDB itemDB;
    public GameObject deathEffect;
    private EnemyAudio enemyAudio;
    private GemDropout gemDropout;
    protected EnemyManager enemyManager;

    protected override void Awake()
    {
        base.Awake();
        enemyManager = FindObjectOfType<EnemyManager>();

        waveSpawner = GameObject.FindObjectOfType<WaveSpawner>();
        itemDB = GameObject.FindObjectOfType<ItemDB>();
        enemyAudio = GetComponent<EnemyAudio>();
        enemyDropout = GetComponent<EnemyDropout>();
        gemDropout = GetComponent<GemDropout>();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        if (!enemyManager.enemies.Contains(this))
        {
            enemyManager.enemies.Add(this);
        }
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        enemyAudio.PlayHitSound();
    }

    protected override void Die()
    {
        enemyAudio.PlayDeadSound();
        animator.Play("Death");
        SpawnDropout();
        SpawnGem();
        photonView.RPC("SpawnEffect", RpcTarget.All);
        PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    private void SpawnEffect()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
    }

    private void OnDestroy()
    {
        if (enemyManager)
        {
            enemyManager.enemies.Remove(this);
        }
    }

    private void SpawnDropout()
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) < enemyDropout.DropoutPossibility)
        {
            switch (enemyDropout.DropoutType)
            {
                case DropoutType.ShootWeapon:
                    PhotonNetwork.Instantiate(itemDB.ShootWeaponDropout.name, transform.position, Quaternion.identity).GetComponent<Dropout>();
                    break;
                case DropoutType.ThrowWeapon:
                    PhotonNetwork.Instantiate(itemDB.ThrowWeaponDropout.name, transform.position, Quaternion.identity).GetComponent<Dropout>();
                    break;
                case DropoutType.MeleeWeapon:
                    PhotonNetwork.Instantiate(itemDB.MeleeWeaponDropout.name, transform.position, Quaternion.identity).GetComponent<Dropout>();

                    break;
                default:
                    break;
            }
        }
    }

    private void SpawnGem()
    {
        EnemyGem[] enemyGems = gemDropout.enemyGems;
        for (int i = 0; i < enemyGems.Length; i++)
        {
            EnemyGem enemyGem = enemyGems[i];
            for (int j = 0; j < enemyGem.count; j++)
            {
                GameObject gem = PhotonNetwork.Instantiate(itemDB.GetGem(enemyGem.index).gameObject.name, transform.position, Quaternion.identity);
                Destroy(gem, 15.0f);
            }
        }
    }
}
