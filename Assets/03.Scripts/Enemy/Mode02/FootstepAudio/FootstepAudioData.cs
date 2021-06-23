using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configure/Footstep Audio Data")]
public class FootstepAudioData : ScriptableObject
{
    public List<FootstepAudio> FootstepAudios = new List<FootstepAudio>();
}

[System.Serializable]
public class FootstepAudio
{
    public string Tag;
    public List<AudioClip> AduioClips = new List<AudioClip>();
    public float Delay;
    //public float SprintingDelay;//跑步的时间间隔
    //public float CrouchingDelay;//下蹲的时间间隔
}
