using UnityEngine;

public class WeaponMegaAudio : WeaponAudio
{
    [SerializeField] private AudioClip chargeAudio;
    [SerializeField] private AudioClip finalAudio;
    [SerializeField] private AudioClip shootChargeAudio;
    [SerializeField] private AudioClip shootFinalAudio;

    public void PlayChargeSound()
    {
        audioSource.clip = chargeAudio;
        audioSource.Play();
    }

    public void PlayFinalSound()
    {
        audioSource.clip = finalAudio;
        audioSource.Play();
    }

    public void PlayShootChargeSound()
    {
        audioSource.clip = shootChargeAudio;
        audioSource.Play();
    }

    public void PlayShootFinalSound()
    {
        audioSource.clip = shootFinalAudio;
        audioSource.Play();
    }
}
