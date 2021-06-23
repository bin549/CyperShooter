using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGunController : MonoBehaviour
{
    [SerializeField] private GameObject leftHandParent;
    [SerializeField] private GameObject rightHandParent;

    private void Update()
    {
        Fire();
        ReleaseMagazine();
    }

    private void Fire()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            var simpleGunScript = CheckIfSimpleGunInHand(rightHandParent);
            if (simpleGunScript != null)
            {
                simpleGunScript.Fire = true;
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            var simpleGunScript = CheckIfSimpleGunInHand(leftHandParent);
            if (simpleGunScript != null)
            {
                simpleGunScript.Fire = true;
            }
        }
    }

    private void ReleaseMagazine()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            var simpleGunScript = CheckIfSimpleGunInHand(rightHandParent);
            if (simpleGunScript != null && simpleGunScript.magazine != null)
            {
                simpleGunScript.ReleaseMagazine = true;
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            var simpleGunScript = CheckIfSimpleGunInHand(leftHandParent);
            if (simpleGunScript != null && simpleGunScript.magazine != null)
            {
                simpleGunScript.ReleaseMagazine = true;
            }
        }
    }

    private SimpleGun CheckIfSimpleGunInHand(GameObject handParent)
    {
        var simpleGunScript = handParent.GetComponentInChildren<SimpleGun>();
        return simpleGunScript;
    }
}
