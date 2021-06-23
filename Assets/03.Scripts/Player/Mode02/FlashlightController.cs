using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] private GameObject leftHandParent;
    [SerializeField] private GameObject rightHandParent;

    private void Update()
    {
        TurnFlashlight();
    }

    private void TurnFlashlight()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            var flashlightScript = CheckIfFlashlightInHand(rightHandParent);
            if (flashlightScript != null)
            {
                flashlightScript.Turn = true;
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            var flashlightScript = CheckIfFlashlightInHand(leftHandParent);
            if (flashlightScript != null)
            {
                flashlightScript.Turn = true;
            }
        }
    }

    private Flashlight CheckIfFlashlightInHand(GameObject handParent)
    {
        var flashlightScript = handParent.GetComponentInChildren<Flashlight>();
        return flashlightScript;
    }
}
