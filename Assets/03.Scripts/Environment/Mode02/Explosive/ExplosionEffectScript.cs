using UnityEngine;
using System.Collections;

public class ExplosionEffectScript : MonoBehaviour
{
    public ConfigurationScript ConfigurationScript;

    [Header("Customizable Options")]
    public float despawnTime = 10.0f, lightDuration = 0.02f;

    [Header("Light")] public Light lightFlash;

    [Header("Audio")]
    private AudioClip[] explosionSounds;
    public AudioSource audioSource;

    private void Start()
    {
        explosionSounds = ConfigurationScript.explosionSounds;
        StartCoroutine(DestroyTimer());
        StartCoroutine(LightFlash());
        audioSource.clip = explosionSounds[Random.Range(0, explosionSounds.Length)];
        audioSource.Play();
    }

    private IEnumerator LightFlash()
    {
        lightFlash.GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(lightDuration);
        lightFlash.GetComponent<Light>().enabled = false;
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
