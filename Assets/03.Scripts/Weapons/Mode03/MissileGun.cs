using UnityEngine;
using Photon.Pun;

public class MissileGun : BulletWeapon
{
    private Transform target;
    [SerializeField] protected Transform m_target;
    [SerializeField] protected OVRInput.Button lockButton = OVRInput.Button.PrimaryHandTrigger;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Vector3 dir;
    [SerializeField] private float spherecastRadius = 0.1f;
    //  [SerializeField] private int targetObjectsInLayer = 0;
    [SerializeField] private float maxTargetDistance = 10.0f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        FindTarget(out target);
        if (target != m_target)
        {
            m_target = target;
        }
        if (m_target != null)
        {
          SeeTarget();
        }
        else
        {
          if (crosshair.activeSelf)
          crosshair.SetActive(false);
        }
        WeaponShoot();
    }

    protected void SeeTarget()
    {
        if (OVRInput.GetDown(lockButton, Controller) || Input.GetKeyDown(KeyCode.Q))
        {
            crosshair.SetActive(true);
            crosshair.transform.position = target.position + dir.normalized;
            crosshair.transform.rotation = Quaternion.LookRotation(dir);
        }
        if (OVRInput.GetUp(lockButton, Controller) || Input.GetKeyUp(KeyCode.Q))
        {
            crosshair.SetActive(false);
        }
    }

    protected void FindTarget(out Transform target)
    {
        target = null;
        Debug.DrawRay(firePoint.position, firePoint.forward, Color.red, 0.1f);

        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hitInfo;
        //int layer = (targetObjectsInLayer == -1) ? ~0 : 1 << targetObjectsInLayer;
        //if (Physics.SphereCast(ray, spherecastRadius, out hitInfo, maxTargetDistance, layer))
        if (Physics.SphereCast(ray, spherecastRadius, out hitInfo, maxTargetDistance))
        {
            Transform enemy_target = null;
            Transform car_target = null;
            if (hitInfo.collider != null)
            {
                var enemyHealth = hitInfo.collider.gameObject.GetComponent<EnemyHealth>() ?? hitInfo.collider.gameObject.GetComponentInParent<EnemyHealth>();
                enemy_target = enemyHealth == null ? null : hitInfo.collider.gameObject.transform;
                if (enemy_target)
                {
                    target = enemy_target;
                }
                var car = hitInfo.collider.gameObject.GetComponent<Car>() ?? hitInfo.collider.gameObject.GetComponentInParent<Car>();
                car_target = car == null ? null : hitInfo.collider.gameObject.transform;
                if (car_target)
                {
                    target = car_target;
                }
            }
        }
    }

    protected override void Shoot()
    {
        weaponAudio.PlayShootSound();
        VibrationManager.Instance.VibrateController(duration, frequency, amplitude, Controller);

        if (m_target != null)
        {
            var bullet_target = m_target;
            MissileBullet missile = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation).GetComponent<MissileBullet>(); ;
            missile.Seek(bullet_target);
        }
        else
        {
            PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
        }
    }
}
