using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioClip healClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    public void SetHealSound(AudioClip healSound)
    {
        this.healClip = healClip;
    }

    public void PlayHealSound()
    {
        audioSource.clip = healClip;
        audioSource.Play();
    }
}
