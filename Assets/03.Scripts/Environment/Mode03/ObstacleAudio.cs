using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObstacleAudio : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip collAudio;
    [SerializeField] private AudioClip destoryAudio;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCollSound()
    {
        audioSource.clip = collAudio;
        audioSource.Play();
    }

    public void PlayDestorySound()
    {
        audioSource.clip = destoryAudio;
        audioSource.Play();
    }
}
