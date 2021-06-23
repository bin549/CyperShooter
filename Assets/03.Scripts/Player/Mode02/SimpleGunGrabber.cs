using UnityEngine;

public class SimpleGunGrabber : CustomGrabber
{
    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller) || Input.GetKeyDown(KeyCode.E))
        {
            if (!hand.isFull)
            {
                var simpleGun = other.transform.root.gameObject.GetComponent<SimpleGun>();
                if (simpleGun != null)
                {
                    if (other.transform.parent != null && other.transform.parent.name.Contains("Snap"))
                    {
                        return;
                    }
                    else
                    {
                        PlaceInHand(other, simpleGun.snapOffset);
                    }
                }
            }
        }
    }
}
