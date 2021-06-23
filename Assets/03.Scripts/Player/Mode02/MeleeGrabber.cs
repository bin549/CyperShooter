using UnityEngine;

public class MeleeGrabber : CustomGrabber
{
    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller) || Input.GetKeyDown(KeyCode.E))
        {
            if (!hand.isFull)
            {
                var meleeGrabbable = other.transform.root.gameObject.GetComponent<MeleeGrabbable>();
                if (meleeGrabbable != null)
                {
                    if (other.transform.parent != null && other.transform.parent.name.Contains("Snap"))
                    {
                        return;
                    }
                    else
                    {
                        PlaceInHand(other, meleeGrabbable.snapOffset);
                    }
                }
            }
        }
    }
}
