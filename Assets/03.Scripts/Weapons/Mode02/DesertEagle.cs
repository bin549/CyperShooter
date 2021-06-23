using UnityEngine;

public class DesertEagle : ComplicateGun, ICanHolster
{
    private float canShootAgainTime = 0.5f;
    public float canShootAgain = 0;
    public int SnapPosition { get; set; } = 0;

    private void Update()
    {
        if (ReleaseMagazine && magazineInGun)
        {
            ReleaseMagazineFromGun();
        }
        if (CockBack && !Cocked)
        {
            CockBackSlider();
        }
        if (Fire)
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        if (Cocked && rounds > 1 && canShootAgain < Time.time)
        {
            canShootAgain = Time.time + canShootAgainTime;
            rounds--;
            animator.SetTrigger("Fire");
            CheckForHit();
            Invoke("KickBack", 0.1f);
            audioSource.PlayOneShot(fireSound);
            if (transform.parent.transform.parent.name == "CustomHandRight")
            {
                VibrationManager.Instance.VibrateController(0.25f, 1f, 1, OVRInput.Controller.RTouch);
            }
            else if (transform.parent.transform.parent.name == "CustomHandLeft")
            {
                VibrationManager.Instance.VibrateController(0.25f, 1f, 1, OVRInput.Controller.LTouch);
            }
        }
        else if (Cocked && rounds == 1 && canShootAgain < Time.time)
        {
            canShootAgain = Time.time + canShootAgainTime;
            rounds--;
            animator.SetTrigger("FireLastRound");
            var bullet = magazine.transform.GetChild(0).gameObject;
            bullet.SetActive(false);
            CheckForHit();
            Invoke("KickBack", 0.1f);
            audioSource.PlayOneShot(fireSound);

            if (transform.parent.transform.parent.name == "CustomHandRight")
            {
                VibrationManager.Instance.VibrateController(0.25f, 1f, 1, OVRInput.Controller.RTouch);
            }
            else if (transform.parent.transform.parent.name == "CustomHandLeft")
            {
                VibrationManager.Instance.VibrateController(0.25f, 1f, 1, OVRInput.Controller.LTouch);
            }
        }
        else if (Cocked && rounds <= 0 && !animator.GetCurrentAnimatorStateInfo(0).IsTag("FireLastRound"))
        {
            Cocked = false;
            animator.SetTrigger("FireNoAmmo");
            audioSource.PlayOneShot(fireNoAmmoSound);
        }
        Fire = false;
    }

    private void KickBack()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 10);
        Invoke("ReturnKickBack", 0.1f);
    }

    private void ReturnKickBack()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 10);
    }
}
