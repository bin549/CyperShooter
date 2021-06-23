using UnityEngine;
using System.Collections;

public class ImpactScript : MonoBehaviour
{
    public float despawnTimer = 10.0f;

    public AudioClip[] impactSounds;
    public AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(DespawnTimer());
        audioSource.clip = impactSounds[Random.Range(0, impactSounds.Length)];
        audioSource.Play();
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnTimer);
        Destroy(gameObject);
    }
}
