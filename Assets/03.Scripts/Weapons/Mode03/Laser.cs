using UnityEngine;
using Photon.Pun;

public class Laser : ShootWeapon
{
    public bool useLaser = false;
    public int damageOverTime = 100;
    public float slowAmount = .5f;

    public LineRenderer aimLineRenderer;

    [SerializeField] private GameObject[] beamStartPrefabs;
    [SerializeField] private GameObject[] beamEndPrefabs;
    [SerializeField] private GameObject[] beamLineRendererPrefabs;

    [SerializeField] private GameObject beamStartPrefab;
    [SerializeField] private GameObject beamEndPrefab;
    [SerializeField] private GameObject beamLineRendererPrefab;

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;

    [SerializeField] private LineRenderer attackLineRenderer;

    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private EnemyHealth targetHealth;
    [SerializeField] private EnemyAI targetMovement;
    [SerializeField] private Obstacle obstacle;
    [SerializeField] private Car car;

    public bool drawLine = false;

    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f;
    public float textureScrollSpeed = 8f;
    public float textureLengthScale = 3;

    private int upgradeBeamIndex = 0;
    [SerializeField] protected OVRInput.Button lockButton = OVRInput.Button.PrimaryHandTrigger;

    protected override void Update()
    {
        base.Update();
        DetectTarget();
        DrawAimingLine();
        Shoot();
    }

    private void DetectTarget()
    {
        Ray ray = new Ray();
        ray.direction = firePoint.forward;
        ray.origin = firePoint.position;

        bool hitCheck = Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit);
        if (hitCheck)
        {
            targetPosition = hit.point;

            bool hitEnemy = hit.collider.gameObject.CompareTag("Enemy");
            bool hitObstacle = hit.collider.gameObject.CompareTag("Obstacle");
            bool hitCar = hit.collider.gameObject.CompareTag("Car");

            if (drawLine)
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.1f);
                aimLineRenderer.enabled = true;
                aimLineRenderer.SetPosition(0, firePoint.position);
                aimLineRenderer.SetPosition(1, hit.point);
                aimLineRenderer.sharedMaterial.color = hitEnemy ? Color.green : Color.white;
            }

            if (hitEnemy)
            {
                targetHealth = hit.collider.gameObject.GetComponent<EnemyHealth>();
                targetMovement = hit.collider.gameObject.GetComponent<EnemyAI>();
            }
            if (hitObstacle)
            {
                obstacle = hit.collider.gameObject.GetComponent<Obstacle>();
            }
            if (hitCar)
            {
                car = hit.collider.gameObject.GetComponent<Car>();
            }
        }
        else
        {
            aimLineRenderer.enabled = false;
        }
    }

    private void DrawAimingLine()
    {
        if (OVRInput.GetDown(lockButton, Controller) || Input.GetKeyDown(KeyCode.Q))
        {
            drawLine = true;
        }
        if (OVRInput.GetUp(lockButton, Controller) || Input.GetKeyUp(KeyCode.Q))
        {
            drawLine = false;
            aimLineRenderer.enabled = false;
        }
    }

    protected override void Shoot()
    {
        if (OVRInput.GetDown(actionButton, Controller) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            photonView.RPC("LockTarget", RpcTarget.All);
        }
        if (OVRInput.GetUp(actionButton, Controller) || Input.GetKeyUp(KeyCode.Mouse0))
        {
            photonView.RPC("UnLockTarget", RpcTarget.All);
        }
        LaserShoot();
    }


    [PunRPC]
    private void LockTarget()
    {
        ResetTarget();
        beamStart = Instantiate(beamStartPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beamEnd = Instantiate(beamEndPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beam = Instantiate(beamLineRendererPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        attackLineRenderer = beam.GetComponent<LineRenderer>();
        aimLineRenderer.enabled = false;
        useLaser = true;
    }

    [PunRPC]
    private void UnLockTarget()
    {
        Destroy(beamStart);
        Destroy(beamEnd);
        Destroy(beam);
        aimLineRenderer.enabled = true;
        useLaser = false;
        VibrationManager.Instance.TurnOffVibrate(controller);

        weaponAudio.Stop();
    }

    private void ResetTarget()
    {
        targetHealth = null;
        targetMovement = null;
        obstacle = null;
    }

    private void LaserShoot()
    {
        if (useLaser)
        {
            beamStart.transform.position = firePoint.position;
            attackLineRenderer.SetPosition(0, firePoint.position);
            attackLineRenderer.SetPosition(1, targetPosition);
            beamEnd.transform.position = targetPosition;
            beamStart.transform.LookAt(beamEnd.transform.position);
            beamEnd.transform.LookAt(beamStart.transform.position);
            float distance = Vector3.Distance(firePoint.position, targetPosition);
            attackLineRenderer.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
            attackLineRenderer.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);

            if (targetMovement != null)
            {
                targetHealth.TakeDamage(damageOverTime * Time.deltaTime);
                targetMovement.Slow(slowAmount);
            }
            if (obstacle != null)
            {
                obstacle.TakeDamage();
            }
            if (car != null)
            {

                car.LaserDamage();

            }
            VibrationManager.Instance.VibrateController(frequency, amplitude, Controller);
            weaponAudio.PlayShootSound();
        }
    }

    public override void UpgradeWeapon()
    {
        base.UpgradeWeapon();
        beamStartPrefab = beamStartPrefabs[upgradeBeamIndex];
        beamEndPrefab = beamEndPrefabs[upgradeBeamIndex];
        beamLineRendererPrefab = beamLineRendererPrefabs[upgradeBeamIndex];
        upgradeBeamIndex++;
    }
}
