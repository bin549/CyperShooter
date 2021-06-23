using UnityEngine;

public class ThrowWeapon : UpgradeWeaponController
{
    protected Rigidbody rigidbody;
    public float throwGain = 1.0f;
    private Vector3 m_anchorOffsetPosition;
    private Quaternion m_anchorOffsetRotation;

    protected void Throw()
    {
        var weaponControllerManager = transform.parent.gameObject.GetComponent<WeaponControllerManager>();
        transform.parent = null;

        m_anchorOffsetPosition = transform.localPosition;
        m_anchorOffsetRotation = transform.localRotation;
        OVRPose localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(controller), orientation = OVRInput.GetLocalControllerRotation(controller) };
        OVRPose offsetPose = new OVRPose { position = m_anchorOffsetPosition, orientation = m_anchorOffsetRotation };
        localPose = localPose * offsetPose;
        OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
        Vector3 linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(controller);
        linearVelocity *= throwGain;
        Vector3 angularVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(controller);
        //  transform.SetParent(transform.parent);

        rigidbody.isKinematic = false;
        rigidbody.velocity = linearVelocity;
        rigidbody.angularVelocity = angularVelocity;

        weaponControllerManager.Remove(this);
        weaponControllerManager.canChangeWeapon = true;
    }
}
