using UnityEngine;

public class Ultima : BombableWeapon
{
    public float delay = 3f;
    private float countdown;
    public bool canExplose = false;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        countdown = delay;
        canExplose = true;
    }

    private void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, controller))
        {
            Throw();
            if (rigidbody.velocity.sqrMagnitude > 3.0f)
            {
                canExplose = true;
            }
        }

        if (canExplose)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                Explode();
                hasExploded = true;
            }
        }
    }
}
