using UnityEngine;

public class SimpleHandGun : SimpleGun
{
    public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        rigidBody.isKinematic = false;
        rigidBody.velocity = linearVelocity;
        rigidBody.angularVelocity = angularVelocity;
        GrabEnd();
    }
}
 