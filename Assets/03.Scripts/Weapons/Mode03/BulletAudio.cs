using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BulletAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip impactAudio;
    public float randomPercent = 10;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.pitch *= 1 + Random.Range(-randomPercent / 100, randomPercent / 100);
    }

    public void PlayImpactSound()
    {
        audioSource.clip = impactAudio;
        audioSource.Play();
    }
}
