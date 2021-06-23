using UnityEngine;
using OVRTouchSample;
using System.Linq;

public class ItemGrabber : CustomGrabber
{
    public float throwGain = 1.0f;

    private Vector3 m_anchorOffsetPosition;
    private Quaternion m_anchorOffsetRotation;
    [SerializeField] protected ItemSnapOffsets itemSnapOffsets;


    protected override void Start()
    {
        base.Start();
        m_anchorOffsetPosition = transform.localPosition;
        m_anchorOffsetRotation = transform.localRotation;
        itemSnapOffsets = GetComponent<ItemSnapOffsets>();
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

                ItemGrabbable throwGrabbable = objectInHand.GetComponent<ItemGrabbable>();
                throwGrabbable.GrabEnd(linearVelocity, angularVelocity);

                transform.parent.GetComponent<WeaponControllerManager>().canChangeWeapon = true;
                hand.isFull = false;
                objectInHand = null;
                handVisual.SetActive(true);
            }
        }
    }

    protected override void OnTriggerStay(Collider other)
    {
        var itemGrabbable = other.transform.root.gameObject.GetComponent<ItemGrabbable>();
        if (itemGrabbable != null)
            itemGrabbable.snapOffset = itemSnapOffsets.snapOffsets.ToList().Find(snapOffset => snapOffset.gameObject.name.Contains(other.gameObject.name.Split('(')[0]));

        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, m_controller) || Input.GetKeyDown(KeyCode.E))
        {
            if (!hand.isFull)
            {
                if (itemGrabbable != null)
                {
                    if (other.transform.parent != null && other.transform.parent.name.Contains("Snap"))
                    {
                        return;
                    }
                    else
                    {
                        transform.parent.GetComponent<WeaponControllerManager>().canChangeWeapon = false;
                        PlaceInHand(other, itemGrabbable.snapOffset);
                    }
                }
            }
        }
    }
}
