using System.Collections;
using UnityEngine;

public class EnemyFirearms : MonoBehaviour, IWeapon
{
    [Range(.0f, 1.0f)] public float spreadAngle;
    public float weaponRange;
    public float weaponDamage;
    public float fireRate;
    public int ammoInMag = 30;
    public Animator characterAnimator;
    public Transform muzzleFlashTransform;
    public LineRenderer bulletLine;
    public MeshRenderer[] muzzleFlashMeshRenderer;
    public AudioSource firearmsShootingAudioSource;
    public MaterialCtrl impactMaterialCtrl;
    private AnimatorStateInfo gunStateInfo;
    private IEnumerator checkerCoroutine;
    private int currentAmmo;
    private float lastFireTime;
    private bool inReload;

    private void Awake()
    {
        inReload = false;
        bulletLine.enabled = false;
        currentAmmo = ammoInMag;
        foreach (MeshRenderer tmpMuzzleFlash in muzzleFlashMeshRenderer)
        {
            tmpMuzzleFlash.enabled = false;
        }
    }

    private void Start()
    {
        bulletLine.SetPosition(0, muzzleFlashTransform.position);
    }

    public void DoAttack()
    {
        if (inReload)
            return;
        Shooting();
        if (currentAmmo <= 0)
            StartCoroutine(Reload());
    }

    private void Shooting()
    {
        if (currentAmmo <= 0)
            return;
        if (!IsAllowShooting())
            return;
        currentAmmo -= 1;
        CreateBullet();
        CheckShowCoroutine(ShowMuzzleFlashCoroutine());
        characterAnimator.Play("Fire", 1, 0.0f);
        firearmsShootingAudioSource.Play();
        lastFireTime = Time.time;
    }

    private void CheckShowCoroutine(IEnumerator BeCheckCoroutine)
    {
        if (checkerCoroutine == null)
        {
            checkerCoroutine = BeCheckCoroutine;
            StartCoroutine(checkerCoroutine);
        }
        else if (checkerCoroutine == BeCheckCoroutine)
        {
            StopCoroutine(checkerCoroutine);
            checkerCoroutine = null;
            checkerCoroutine = BeCheckCoroutine;
            StartCoroutine(checkerCoroutine);
        }
        else
        {
            checkerCoroutine = null;
            checkerCoroutine = BeCheckCoroutine;
            StartCoroutine(checkerCoroutine);
        }
    }

    private IEnumerator Reload()
    {
        inReload = true;
        characterAnimator.Play("Reload", 1, 0.0f);

        while (true)
        {
            yield return null;
            gunStateInfo = characterAnimator.GetCurrentAnimatorStateInfo(1);
            if (gunStateInfo.IsTag("Reload"))
            {
                if (gunStateInfo.normalizedTime >= 0.98f)
                {
                    currentAmmo = ammoInMag;
                    inReload = false;
                    yield break;
                }
            }
        }
    }

    private void CreateBullet()
    {
        bool tmphit = Physics.Raycast(muzzleFlashTransform.position, (muzzleFlashTransform.forward + CalculateSpreadOffset()).normalized, out RaycastHit hit, weaponRange);
        if (!tmphit) return;
        if (hit.collider)
        {
            foreach (var tmpImpactEffect in impactMaterialCtrl.ImpactMaterialData)
            {
                if (hit.collider.CompareTag(tmpImpactEffect.Tag))
                {
                    GameObject tmpBlood = Instantiate(tmpImpactEffect.ImpactHitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    tmpBlood.transform.SetParent(hit.collider.transform);
                }
            }
            if (hit.collider.GetComponentInParent<Health>())
            {
                hit.collider.GetComponentInParent<Health>().TakeDamage(weaponDamage, null);
            }
        }
        if (Random.value > 0.5f)
        {
            CheckShowCoroutine(CreateBulleyLineCroutine(muzzleFlashTransform.position, hit.point));
        }
        else
        {
            bulletLine.enabled = false;
        }
    }

    private IEnumerator CreateBulleyLineCroutine(Vector3 StartPosition, Vector3 EndPosition)
    {
        bulletLine.enabled = true;
        Vector3 tmpvector3 = EndPosition - StartPosition;
        bulletLine.SetPosition(0, StartPosition + tmpvector3 * Random.value);
        bulletLine.SetPosition(1, EndPosition - tmpvector3 * Random.value);
        yield return new WaitForSeconds(Random.Range(0.01f, 0.05f));
        bulletLine.enabled = false;
    }

    private Vector3 CalculateSpreadOffset()
    {
        return spreadAngle * UnityEngine.Random.insideUnitCircle;
    }

    private bool IsAllowShooting()
    {
        return Time.time - lastFireTime > 1 / fireRate;
    }

    private IEnumerator ShowMuzzleFlashCoroutine()
    {
        float tmpscale = Random.Range(0.5f, 1.5f);
        muzzleFlashTransform.localScale = Vector3.one * tmpscale;
        Quaternion tmprot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        muzzleFlashTransform.localRotation = tmprot;
        foreach (MeshRenderer tmpMuzzleFlash in muzzleFlashMeshRenderer)
        {
            tmpMuzzleFlash.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.01f, 0.05f));
            tmpMuzzleFlash.enabled = false;
        }
    }
}
