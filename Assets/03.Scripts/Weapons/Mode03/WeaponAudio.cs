using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponAudio : MonoBehaviour
{
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip shootAudio;
    [SerializeField] protected AudioClip upgradeAudio;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayShootSound()
    {
        audioSource.clip = shootAudio;
        audioSource.Play();
    }

    public void PlayUpgradeSound()
    {
        audioSource.clip = upgradeAudio;
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
