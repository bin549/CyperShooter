using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Shield : WeaponController
{
    private Animator animator;
    private bool isZoomOut;
    public AudioSource audioSource;
    public AudioClip bounceSound;
    public float bounceForce;
    public ForceMode forceMode;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddRelativeForce(-collision.gameObject.transform.forward * bounceForce, forceMode);
            VibrationManager.Instance.VibrateController(0.1f, 2.9f, Controller);
            //        AudioManager.Instance.Play(audioSource, bounceSound);
        }
    }
}
