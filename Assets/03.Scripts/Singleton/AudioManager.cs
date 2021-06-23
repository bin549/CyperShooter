using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip bgmClip;
    public AudioClip gameWinClip;
    public AudioClip gameOverClip;
    public AudioClip gameLoadClip;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = bgmClip;
    }

    private void Start()
    {
        audioSource.Play();
    }

    public void Win()
    {
        audioSource.loop = false;
        audioSource.clip = gameWinClip;
        audioSource.Play();
    }

    public void Load()
    {
        audioSource.loop = false;
        audioSource.clip = gameLoadClip;
        audioSource.Play();
    }

    public void Lose()
    {
        audioSource.loop = false;
        audioSource.clip = gameOverClip;
        audioSource.Play();
    }

    public void DoMute(bool isMute)
    {
        if (isMute)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
    }

    public void Resume()
    {
        audioSource.Play();
    }

    public void Play(AudioSource audioSource, AudioClip audioClip, bool isLoop = false)
    {
        if (isLoop)
            audioSource.loop = true;
        else
            audioSource.loop = false;

        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void Stop(AudioSource audioSource)
    {
        audioSource.Stop();
    }
}
