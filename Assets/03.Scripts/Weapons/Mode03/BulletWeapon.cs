using UnityEngine;
using Photon.Pun;

public class BulletWeapon : ShootWeapon
{
    protected float elapsedTime;
    [SerializeField] private ShootBullet[] bullets;
    public ShootBullet bulletPrefab;
    private int nextBulletIndex = 0;
    public float fireRate = 0.1f;
    public float upgradeFireRate = 0.02f;

    [SerializeField] protected bool willColorful;
    [SerializeField] protected bool isColorful;
    [SerializeField] protected bool willSMG;
    [SerializeField] protected bool isSMG;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        WeaponShoot();
    }

    protected void WeaponShoot()
    {
        elapsedTime += Time.deltaTime;
        if (!isSMG)
        {
            if (OVRInput.GetDown(actionButton, Controller) || Input.GetKeyDown(KeyCode.E))
            {
                if (elapsedTime > fireRate)
                {
                    Shoot();
                    elapsedTime = 0;
                }
            }
        }
        else
        {
            if (OVRInput.Get(actionButton, Controller)|| Input.GetKey(KeyCode.E))
            {
                if (elapsedTime > fireRate)
                {
                    Shoot();
                    elapsedTime = 0;
                }
            }
        }
    }

    public override void UpgradeWeapon()
    {
        base.UpgradeWeapon();
        if (bullets[nextBulletIndex] != null)
            bulletPrefab = bullets[nextBulletIndex];
        nextBulletIndex++;
        if (nextBulletIndex == bullets.Length / 2)
        {
            if (willSMG)
                isSMG = true;
        }
        if (nextBulletIndex == bullets.Length)
        {
            {
                if (willColorful)
                    isColorful = true;
            }
        }
        fireRate -= upgradeFireRate;
    }

    protected override void Shoot()
    {
        weaponAudio.PlayShootSound();
        VibrationManager.Instance.VibrateController(duration, frequency, amplitude, Controller);

        if (isColorful)
        {
            bulletPrefab = bullets[Random.Range(0, bullets.Length)];
        }
        PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
    }
}
