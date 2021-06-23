using UnityEngine;

public class ThrowGrabbable : CustomGrabbable
{
    public bool canExplose = false;

    public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        rigidBody.isKinematic = false;
        rigidBody.velocity = linearVelocity;
        rigidBody.angularVelocity = angularVelocity;
        if (rigidBody.velocity.sqrMagnitude > 3.0f)
        {
            canExplose = true;
        }
        GrabEnd();
    }
}
