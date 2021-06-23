using UnityEngine;

public class ComplicateGun : Gun
{
    [SerializeField] private bool cockBack = false;
    [SerializeField] private bool cocked = false;
    public bool CockBack { get => cockBack; set => cockBack = value; }
    public bool Cocked { get => cocked; set => cocked = value; }
    [SerializeField] protected GameObject cartridge;
    [SerializeField] protected bool isDesertEagle;
    [SerializeField] protected AudioClip cockBackSound;
    [SerializeField] public Transform cockBackPosition;
    [SerializeField] public Transform cartridgeExit;

    protected override void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(magazineTag))
        {
            if (!magazineInGun)
            {
                audioSource.PlayOneShot(magazineInSound);
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
                if (animator.GetCurrentAnimatorStateInfo(0).IsTag("FireLastRound"))
                {
                    animator.SetTrigger("Loaded");
                }
            }
        }
    }


    public void CockBackSlider()
    {
        if (isDesertEagle)
            Cocked = true;
        audioSource.PlayOneShot(cockBackSound);
        CockBack = false;
        animator.SetTrigger("CockBack");

        if (transform.parent.transform.parent.name == "CustomHandRight")
        {
            VibrationManager.Instance.VibrateController(0.25f, 0.1f, 0.1f, OVRInput.Controller.RTouch);
        }
        else if (transform.parent.transform.parent.name == "CustomHandLeft")
        {
            VibrationManager.Instance.VibrateController(0.25f, 0.1f, 0.1f, OVRInput.Controller.LTouch);
        }
    }

    public void SendCartridge()
    {
        if (!isDesertEagle)
            rounds--;
        var newCartridge = Instantiate(cartridge, cartridgeExit.position, cartridgeExit.rotation);
        var rigidbody = newCartridge.GetComponent<Rigidbody>();
        rigidbody.AddForce(newCartridge.transform.up * 5, ForceMode.Impulse);
        var random = Random.Range(-90, 90);
        var randomTorque = new Vector3(random, random, random);
        rigidbody.AddTorque(randomTorque, ForceMode.Impulse);
        Destroy(newCartridge, 5f);
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

            if (isDesertEagle)
            {
                if (rounds > 1)
                {
                    rounds = 1;
                }
            }
            else
            {
                rounds = 0;
            }
            magazineInGun = false;
            magazine = null;
        }
    }
}
