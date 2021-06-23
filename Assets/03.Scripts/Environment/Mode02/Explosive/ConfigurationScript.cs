using UnityEngine;

[CreateAssetMenu(menuName = "ConfigurationScriptData")]
public class ConfigurationScript : ScriptableObject
{
    public Transform ExplodePrefab;
    public float minTime, maxTime;
    public float explosionRadius;
    public float explosionForce;
    public AudioClip[] debrisSounds;
    public AudioClip[] explosionSounds;
}
