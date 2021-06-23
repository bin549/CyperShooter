using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasTank_Script : ExplosiveBarrel_Script
{
    [Header("爆炸前会乱动模块")]
    public Light lightObject;
    public ParticleSystem flameParticles;
    public ParticleSystem smokeParticles;
    public AudioSource flameSound;
    public AudioSource impactSound;

    public float explosionTimer = 6f, rotationSpeed = 15f, maxRotationSpeed = 25f, moveSpeed = 2500, audioPitchIncrease = 0.25f;
    private bool expload, audioHasPlayed = false;
    private float randomRotationValue, randomValue;

    protected override void Start()
    {
        lightObject.intensity = 0;
        randomValue = UnityEngine.Random.Range(-50, 50);
        base.Start();
    }

    public override void OnDie()
    {
        flameParticles.Play();
        smokeParticles.Play();
        StartCoroutine(FlyGasTank());
        if (!audioHasPlayed)
        {
            flameSound.Play();
            audioHasPlayed = true;
        }
        Invoke("toexplosiveBarrel", explosionTimer);
        lightObject.intensity = 3;
    }

    private IEnumerator FlyGasTank()
    {
        while (true)
        {
            yield return null;
            randomRotationValue += 1.0f * Time.deltaTime;

            if (randomRotationValue > maxRotationSpeed)
            {
                randomRotationValue = maxRotationSpeed;
            }
            gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.down * moveSpeed * 50 * Time.deltaTime);

            transform.Rotate(randomRotationValue, 0, randomValue * rotationSpeed * Time.deltaTime);

            flameSound.pitch += audioPitchIncrease * Time.deltaTime;
            if (expload)
            {
                yield break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        impactSound.Play();
    }

    private void toexplosiveBarrel()
    {
        expload = true;
        base.OnDie();
    }
}
