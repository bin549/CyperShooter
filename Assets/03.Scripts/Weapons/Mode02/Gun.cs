using UnityEngine;

public class Gun : CustomGrabbable
{
    protected Animator animator;

    [SerializeField] public Transform barrelExit;
    [SerializeField] public Transform magazinePosition;

    [Header("Sound")]
    public AudioSource audioSource;
    [SerializeField] protected AudioClip fireSound;
    [SerializeField] protected AudioClip fireNoAmmoSound;
    [SerializeField] protected AudioClip magazineReleaseSound;
    [SerializeField] protected AudioClip magazineInSound;

    public GameObject magazine { get; set; }

    protected bool magazineInGun = false;

    public bool Fire { get; set; } = false;
    public bool ReleaseMagazine { get; set; } = false;
    public bool CanHolster { get; set; } = true;

    [SerializeField] public int rounds = 0;
    [SerializeField] protected GameObject bulletImpact;
    [SerializeField] protected string magazineTag;

    public float weaponDamage = 4.0f;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        animator = GetComponent<Animator>();
    }

    public virtual void Shoot() { }

    protected virtual void ReleaseMagazineFromGun() { }

    protected virtual void OnTriggerEnter(Collider collision) { }

    protected void CheckForHit()
    {
        var forward = barrelExit.transform.TransformDirection(Vector3.forward);
        RaycastHit hitInfo;
        bool hittingSomething = Physics.Raycast(barrelExit.position, forward, out hitInfo, 1000);
        if (hittingSomething)
        {
            if (hitInfo.transform.CompareTag("Actor") || hitInfo.transform.root.CompareTag("Actor"))
            {
                var targetScript = hitInfo.transform.gameObject.GetComponent<Health>() ?? hitInfo.transform.root.gameObject.GetComponent<Health>();
                if (targetScript == null) return;
                targetScript.HitAction(hitInfo.point);
                targetScript.TakeDamage(weaponDamage, null);
            }
            var newImpact = Instantiate(bulletImpact, hitInfo.point, hitInfo.collider.transform.rotation);
            newImpact.transform.forward = hitInfo.normal;
            newImpact.transform.parent = hitInfo.transform;
        }
    }
}
