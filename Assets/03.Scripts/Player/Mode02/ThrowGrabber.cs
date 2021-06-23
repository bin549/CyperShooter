using UnityEngine;
using OVRTouchSample;

public class ThrowGrabber : CustomGrabber
{
    public float throwGain = 1.5f;

    private Vector3 m_anchorOffsetPosition;
    private Quaternion m_anchorOffsetRotation;

    protected override void Start()
    {
        base.Start();
        m_anchorOffsetPosition = transform.localPosition;
        m_anchorOffsetRotation = transform.localRotation;
    }

    protected override void DropItem()
    {
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, m_controller) || Input.GetKeyUp(KeyCode.E))
        {
            if (objectInHand != null)
            {
                objectInHand.transform.parent = null;

                OVRPose localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(m_controller), orientation = OVRInput.GetLocalControllerRotation(m_controller) };
                OVRPose offsetPose = new OVRPose { position = m_anchorOffsetPosition, orientation = m_anchorOffsetRotation };
                localPose = localPose * offsetPose;

                OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
                Vector3 linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(m_controller);
                linearVelocity *= throwGain;
                Vector3 angularVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(m_controller);

                ThrowGrabbable throwGrabbable = objectInHand.GetComponent<ThrowGrabbable>();
                throwGrabbable.GrabEnd(linearVelocity, angularVelocity);

                hand.isFull = false;
                objectInHand = null;
                handVisual.SetActive(true);
            }
        }
    }

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, m_controller) || Input.GetKeyDown(KeyCode.E))
        {
            if (!hand.isFull)
            {
                var throwGrabbable = other.transform.root.gameObject.GetComponent<ThrowGrabbable>();
                if (throwGrabbable != null)
                {
                    if (other.transform.parent != null && other.transform.parent.name.Contains("Snap"))
                    {
                        return;
                    }
                    else
                    {
                        PlaceInHand(other, throwGrabbable.snapOffset);
                    }
                }
            }
        }
    }
}