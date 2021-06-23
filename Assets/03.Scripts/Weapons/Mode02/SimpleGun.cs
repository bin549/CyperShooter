using UnityEngine;

public class SimpleGun : Gun
{
    protected float canShootAgainTime = 0.5f;
    public float canShootAgain = 0;

    protected void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        magazineTag = "Magazine";
    }

    protected void Update()
    {
        if (ReleaseMagazine && magazineInGun)
        {
            ReleaseMagazineFromGun();
        }
        if (Fire)
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        if (rounds > 0 && canShootAgain < Time.time)
        {
            canShootAgain = Time.time + canShootAgainTime;
            rounds--;
            CheckForHit();
            audioSource.PlayOneShot(fireSound);
            if (transform.parent.transform.parent.name == "CustomHandRight")
            {
                VibrationManager.Instance.VibrateController(0.25f, 1f, 1, OVRInput.Controller.RTouch);
            }
            else if (transform.parent.transform.parent.name == "CustomHandLeft")
            {
                VibrationManager.Instance.VibrateController(0.25f, 1f, 1, OVRInput.Controller.LTouch);
            }
            Fire = false;
        }
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(magazineTag) && collision.gameObject.name.Contains(gameObject.name))
        {
            if (!magazineInGun)
            {
                audioSource.PlayOneShot(magazineInSound);
                magazineInGun = true;
                magazine = collision.gameObject;
                var rigidBody = magazine.GetComponent<Rigidbody>();
                rigidBody.isKinematic = true;
                magazine.tag = "UsedMagazine";
                var collider = magazine.GetComponent<Collider>();
                collider.enabled = false;
                magazine.transform.parent = magazinePosition;
                magazine.transform.position = magazinePosition.position;
                magazine.transform.rotation = magazinePosition.rotation;
                magazineInGun = true;
                rounds += 9;
            }
        }
    }

    protected override void ReleaseMagazineFromGun()
    {
        ReleaseMagazine = false;
        if (magazine != null)
        {
            audioSource.PlayOneShot(magazineReleaseSound);
            magazine.transform.parent = null;
            var rigidBody = magazine.GetComponent<Rigidbody>();
            rigidBody.isKinematic = false;
            rigidBody.AddForce(-magazine.transform.up * 3, ForceMode.Impulse);
            magazineInGun = false;
            magazine = null;
        }
    }
}
