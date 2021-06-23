using UnityEngine;

public class Uzi : ComplicateGun, ICanHolster
{
    public bool autoFiring { get; set; } = false;
    public bool stopAutoFiring { get; set; } = true;

    [SerializeField] private AudioClip fireFullAuto;

    private float nextFire = 0;
    private float autoFireSpeed = 0.1f;
    private float spread = 2f;
    public int SnapPosition { get; set; } = 1;

    private void Update()
    {
        if (Fire)
        {
            Fire = false;
            Shoot();
        }

        if (autoFiring)
        {
            AutoFire();
        }
        else if (stopAutoFiring && !autoFiring && animator.GetCurrentAnimatorStateInfo(0).IsTag("Fire"))
        {
            stopAutoFiring = false;
            animator.SetBool("StopFire", true);
            if (!Fire)
            {
                audioSource.Stop();
                if (transform.parent != null && transform.parent.transform.parent.name == "CustomHandRight")
                {
                    VibrationManager.Instance.TurnOffVibrate(OVRInput.Controller.RTouch);
                }
                else if (transform.parent != null && transform.parent.transform.parent.name == "CustomHandLeft")
                {
                    VibrationManager.Instance.TurnOffVibrate(OVRInput.Controller.LTouch);
                }
            }
        }

        if (!Cocked && CockBack)
        {
            CockBack = false;
            CockBackSlider();
        }

        if (ReleaseMagazine)
        {
            ReleaseMagazine = false;
            ReleaseMagazineFromGun();
        }
    }

    private void AutoFire()
    {
        if (rounds <= 0)
        {
            autoFiring = false;
            audioSource.Stop();
        }
        if (rounds > 0 && nextFire < Time.time)
        {
            nextFire = Time.time + autoFireSpeed;
            var spreadx = UnityEngine.Random.Range(-spread, spread);
            var spready = UnityEngine.Random.Range(-spread, spread);
            var spreadz = UnityEngine.Random.Range(-spread, spread);
            barrelExit.transform.rotation = Quaternion.Euler(
                barrelExit.transform.eulerAngles.x + spreadx,
                barrelExit.transform.eulerAngles.y + spready,
                barrelExit.transform.eulerAngles.z + spreadz);
            SendCartridge();
            CheckForHit();
            barrelExit.transform.localRotation = Quaternion.Euler(0, 180, 0);
            animator.SetTrigger("Fire");
            animator.SetBool("StopFire", false);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(fireFullAuto);
            }
            if (transform.parent.transform.parent.name == "CustomHandRight")
            {
                VibrationManager.Instance.VibrateController(0.1f, 1, OVRInput.Controller.RTouch);
            }
            else if (transform.parent.transform.parent.name == "CustomHandLeft")
            {
                VibrationManager.Instance.VibrateController(0.1f, 1, OVRInput.Controller.LTouch);
            }
        }
    }

    public override void Shoot()
    {
        if (Cocked && rounds > 0 && magazineInGun)
        {
            if (Fire)
            {
                SendCartridge();
                CheckForHit();
                animator.SetTrigger("Fire");
                animator.SetBool("StopFire", true);
                audioSource.PlayOneShot(fireSound);
                if (transform.parent.transform.parent.name == "CustomHandRight")
                {
                    VibrationManager.Instance.VibrateController(0.25f, 0.4f, 0.8f, OVRInput.Controller.RTouch);
                }
                else if (transform.parent.transform.parent.name == "CustomHandLeft")
                {
                    VibrationManager.Instance.VibrateController(0.25f, 0.4f, 0.8f, OVRInput.Controller.LTouch);
                }
            }
            else if (!Fire)
            {
                audioSource.Stop();
                autoFiring = true;
            }
        }
        else if (Cocked && rounds <= 0)
        {
            Cocked = false;
            audioSource.Stop();
            audioSource.PlayOneShot(fireNoAmmoSound);
            animator.SetTrigger("FireNoAmmo");
        }
    }
}
