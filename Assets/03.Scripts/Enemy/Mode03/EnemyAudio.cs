using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip dieClip;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip strongAttackClip;

    public AudioClip walkAudioClip;
    public AudioClip runAudioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHitSound()
    {
        audioSource.clip = hitClip;
        audioSource.Play();
    }

    public void PlayAttackSound()
    {
        audioSource.clip = attackClips[Random.Range(0, attackClips.Length)];
        audioSource.Play();
    }

    public void PlayStrongAttackSound()
    {
        audioSource.clip = strongAttackClip;
        audioSource.Play();
    }

    public void PlayDeadSound()
    {
        audioSource.clip = dieClip;
        audioSource.Play();
    }


    public void PlayWalkSound()
    {
        audioSource.clip = walkAudioClip;
        audioSource.Play();
    }


    public void PlayRunSound()
    {
        audioSource.clip = runAudioClip;
        audioSource.Play();
    }
}
