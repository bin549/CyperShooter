using UnityEngine;

public class DebrisScript : MonoBehaviour
{
    [Header("产生的碎片与其他物品碰撞发出声音")]
    private AudioClip[] debrisSounds;
    public AudioSource audioSource;
    public ConfigurationScript ConfigurationScript;

    private void OnCollisionEnter(Collision collision)
    {
        debrisSounds = ConfigurationScript.debrisSounds;
        if (collision.relativeVelocity.magnitude > 50)
        {
            audioSource.clip = debrisSounds[Random.Range(0, debrisSounds.Length)];
            audioSource.Play();
        }
    }
}
