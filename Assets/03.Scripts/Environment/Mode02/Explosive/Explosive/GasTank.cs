using UnityEngine;

[RequireComponent(typeof(ExplosiveBarrel))]
public class GasTank : MonoBehaviour
{
    [Header("爆炸前会乱动模块")]
    public bool isHit = false;
    public float explosionTimer = 6f;
    public float rotationSpeed = 15f;
    public float maxRotationSpeed = 25f;
    public float moveSpeed = 2500;
    public float audioPitchIncrease = 0.25f;

    public Light lightObject;
    public ParticleSystem flameParticles;
    public ParticleSystem smokeParticles;
    public AudioSource flameSound;
    public AudioSource impactSound;

    private bool audioHasPlayed = false;
    private float randomRotationValue;
    private float randomValue;
    private bool notSecondExplod;

    private ExplosiveBarrel explosiveBarrel;
    private void Start()
    {
        notSecondExplod = true;
        lightObject.intensity = 0;
        randomValue = UnityEngine.Random.Range(-50, 50);
        explosiveBarrel = GetComponent<ExplosiveBarrel>();
    }

    private void Update()
    {
        if (isHit == true)
        {
            randomRotationValue += 1.0f * Time.deltaTime;

            if (randomRotationValue > maxRotationSpeed)
            {
                randomRotationValue = maxRotationSpeed;
            }

            gameObject.GetComponent<Rigidbody>().AddRelativeForce
                (Vector3.down * moveSpeed * 50 * Time.deltaTime);


            transform.Rotate(randomRotationValue, 0, randomValue *
                              rotationSpeed * Time.deltaTime);

            flameParticles.Play();

            smokeParticles.Play();
            smokeParticles.Play();
            //会有快爆炸了声效
            flameSound.pitch += audioPitchIncrease * Time.deltaTime;

            if (!audioHasPlayed)
            {
                flameSound.Play();

                audioHasPlayed = true;
            }

            if (notSecondExplod == true)
            {
                Invoke("toexplosiveBarrel", explosionTimer);
                notSecondExplod = false;
                lightObject.intensity = 3;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        impactSound.Play();
    }

    private void toexplosiveBarrel()
    {
        explosiveBarrel.isHit = true;
    }
}
