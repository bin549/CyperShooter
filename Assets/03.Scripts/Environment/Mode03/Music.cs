using UnityEngine;

[System.Serializable]
public class Music
{
    public AudioClip audioClip;
    [Range(0, 20)] public int duration;
}
