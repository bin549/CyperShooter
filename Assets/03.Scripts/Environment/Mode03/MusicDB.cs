using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicDB : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private Music[] musics;
    [SerializeField] private Music currentMusic;
    [SerializeField] private int currentMusicIndex;
    [SerializeField] private int dayCounter = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentMusic = musics[currentMusicIndex];
        //SetCurrentMusic(currentMusic.audioClip);
    }

    public void Play()
    {
        SetCurrentMusic(currentMusic.audioClip);
    }

    public void Tomorrow()
    {
        dayCounter++;
        if (dayCounter == currentMusic.duration && currentMusicIndex != musics.Length)
        {
            currentMusicIndex++;
            currentMusic = musics[currentMusicIndex];
            if (currentMusic.audioClip != null)
                SetCurrentMusic(currentMusic.audioClip);
            dayCounter = 0;
        }
    }

    private void SetCurrentMusic(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
