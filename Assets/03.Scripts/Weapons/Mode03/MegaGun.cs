using UnityEngine;
using Photon.Pun;

public class MegaGun : BulletWeapon
{
    public enum MegaState
    {
        BeginState,
        ChargeState,
        FinalState,
    };

    [SerializeField] private float durationTime;
    [SerializeField] private float chargeBeginTime;
    [SerializeField] private float chargeOverTime;

    public Transform chargePoint;
    public Transform finalEffectPoint;

    [SerializeField] private GameObject[] chargeBulletPrefabs;
    [SerializeField] private GameObject[] finalBulletPrefabs;
    [SerializeField] private GameObject[] chargeEffectPrefabs;
    [SerializeField] private GameObject[] finalEffectPrefabs;

    [SerializeField] private GameObject chargeBulletPrefab;
    [SerializeField] private GameObject finalBulletPrefab;
    [SerializeField] private GameObject chargeEffectPrefab;
    [SerializeField] private GameObject finalEffectPrefab;

    private GameObject chargeBullet;
    private GameObject finalBullet;
    private GameObject chargeEffect;
    private GameObject finalEffect;

    protected WeaponMegaAudio weaponMegaAudio;

    [SerializeField] private MegaState megaState;

    private int WeaponUpgradeIndex = 0;

    private void Start()
    {
        ResetState();
    }

    protected override void Awake()
    {
        base.Awake();
        weaponMegaAudio = GetComponent<WeaponMegaAudio>();
        meshRenderer = GetComponent<MeshRenderer>();
        weaponStats = GetComponent<WeaponStats>();
    }

    protected void Update()
    {
        base.Update();
        Shoot();
    }

    protected override void Shoot()
    {
        elapsedTime += Time.deltaTime;
        if (OVRInput.GetDown(actionButton, Controller) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            VibrationManager.Instance.VibrateController(0.10f, 0.04f, 0.4f, Controller);
        }
        else if (OVRInput.Get(actionButton, Controller) || Input.GetKey(KeyCode.Mouse0))
        {
            if (megaState != MegaState.FinalState)
            {
                ChangeState();
            }
        }
        else if (OVRInput.GetUp(actionButton, Controller) || Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (elapsedTime > fireRate)
            {
                Shooting();
                durationTime = 0;
                elapsedTime = 0;
                ResetState();
            }
        }
    }

    private void ChangeState()
    {
        durationTime += Time.deltaTime;
        switch (megaState)
        {
            case MegaState.BeginState:
                if (durationTime > chargeBeginTime)
                {
                    photonView.RPC("SpawnChargeEffect", RpcTarget.All);
                    megaState = MegaState.ChargeState;
                }
                break;
            case MegaState.ChargeState:
                if (durationTime > chargeOverTime)
                {
                    photonView.RPC("SpawnFinalEffect", RpcTarget.All);
                    megaState = MegaState.FinalState;
                }
                break;
            default:
                break;
        }
    }

    private void ResetState()
    {
        megaState = MegaState.BeginState;
    }

    private void Shooting()
    {
        switch (megaState)
        {
            case MegaState.BeginState:
                photonView.RPC("SpawnBullet", RpcTarget.All);
                break;
            case MegaState.ChargeState:
                photonView.RPC("SpawnChargeBullet", RpcTarget.All);
                break;
            case MegaState.FinalState:
                photonView.RPC("SpawnFinalBullet", RpcTarget.All);
                break;
            default:
                break;
        }
        ResetState();
    }

    [PunRPC]
    private void SpawnChargeEffect()
    {
        chargeEffect = Instantiate(chargeEffectPrefab, chargePoint.position, chargePoint.rotation) as GameObject;
        chargeEffect.transform.parent = chargePoint;
        weaponMegaAudio.PlayChargeSound();
    }

    [PunRPC]
    private void SpawnFinalEffect()
    {
        Destroy(chargeEffect);
        finalEffect = Instantiate(finalEffectPrefab, finalEffectPoint.position, finalEffectPoint.rotation) as GameObject;
        finalEffect.transform.parent = firePoint;
        weaponMegaAudio.PlayFinalSound();
    }


    [PunRPC]
    private void SpawnBullet()
    {
        PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
        weaponMegaAudio.PlayShootSound();
        VibrationManager.Instance.VibrateController(0.15f, 0.1f, 1.3f, Controller);
    }

    [PunRPC]
    private void SpawnChargeBullet()
    {
        Destroy(chargeEffect);
        PhotonNetwork.Instantiate(chargeBulletPrefab.name, firePoint.position, firePoint.rotation);
        weaponMegaAudio.PlayShootChargeSound();
        VibrationManager.Instance.VibrateController(0.15f, 0.13f, 1.8f, Controller);
    }

    [PunRPC]
    private void SpawnFinalBullet()
    {
        Destroy(finalEffect);
        PhotonNetwork.Instantiate(finalBulletPrefab.name, firePoint.position, firePoint.rotation);
        weaponMegaAudio.PlayShootFinalSound();
        VibrationManager.Instance.VibrateController(0.15f, 0.14f, 2.4f, Controller);
    }


    public override void UpgradeWeapon()
    {
        base.UpgradeWeapon();
        chargeBulletPrefab = chargeBulletPrefabs[WeaponUpgradeIndex];
        finalBulletPrefab = finalBulletPrefabs[WeaponUpgradeIndex];
        chargeEffectPrefab = chargeEffectPrefabs[WeaponUpgradeIndex];
        finalEffectPrefab = finalEffectPrefabs[WeaponUpgradeIndex];
        WeaponUpgradeIndex++;
    }
}
