using UnityEngine;

public class ComplicateGunController : MonoBehaviour
{
    [SerializeField] private GameObject leftHandParent;
    [SerializeField] private GameObject rightHandParent;
    [SerializeField] private GameObject leftHandModel;
    [SerializeField] private GameObject rightHandModel;

    private HandAnimationController handAnimationController;

    private void Start()
    {
        handAnimationController = GetComponent<HandAnimationController>();
    }

    private void Update()
    {
        Fire();
        StopFullAutoFire();
        ReleaseMagazine();
        CockBack();
        ToggleFire();
    }

    private void StopFullAutoFire()
    {
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            var uziScript = CheckIfUziInHand(rightHandParent);
            if (uziScript != null)
            {
                handAnimationController.SetRightReleaseTriggerUzi();
                uziScript.autoFiring = false;
                uziScript.stopAutoFiring = true;
            }
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            var uziScript = CheckIfUziInHand(leftHandParent);
            if (uziScript != null)
            {
                handAnimationController.SetLeftReleaseTriggerUzi();
                uziScript.autoFiring = false;
                uziScript.stopAutoFiring = true;
            }
        }
    }

    private void ToggleFire()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            var UziScriptLeftHand = CheckIfUziInHand(leftHandParent);

            if (UziScriptLeftHand != null)
            {
                if (UziScriptLeftHand.Fire)
                {
                    UziScriptLeftHand.Fire = false;
                }
                else
                {
                    VibrationManager.Instance.TurnOffVibrate(OVRInput.Controller.LTouch);
                    UziScriptLeftHand.audioSource.Stop();
                    UziScriptLeftHand.Fire = true;
                    UziScriptLeftHand.stopAutoFiring = true;
                    UziScriptLeftHand.autoFiring = false;
                }
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            var UziScriptRightHand = CheckIfUziInHand(rightHandParent);

            if (UziScriptRightHand.Fire)
            {
                UziScriptRightHand.Fire = false;
            }
            else
            {
                VibrationManager.Instance.TurnOffVibrate(OVRInput.Controller.RTouch);
                UziScriptRightHand.audioSource.Stop();
                UziScriptRightHand.Fire = true;
                UziScriptRightHand.stopAutoFiring = true;
                UziScriptRightHand.autoFiring = false;
            }
        }
    }

    private void CockBack()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            var DeagleScriptLeftHand = CheckIfDeagleInHand(leftHandParent);
            var DeagleScriptRightHand = CheckIfDeagleInHand(rightHandParent);
            if (DeagleScriptLeftHand == null && DeagleScriptRightHand != null)
            {
                var distanceToCockBackPosition = Vector3.Distance(DeagleScriptRightHand.cockBackPosition.position, leftHandModel.transform.position);

                if (distanceToCockBackPosition < 0.1f && !DeagleScriptRightHand.Cocked)
                {
                    handAnimationController.SetLeftCockBack(true);
                    DeagleScriptRightHand.CockBack = true;
                    leftHandModel.transform.parent = DeagleScriptRightHand.cockBackPosition;
                    Invoke("ResetLeftHandModel", 0.5f);
                }
            }
            var UziScriptLeftHand = CheckIfUziInHand(leftHandParent);
            var UziScriptRightHand = CheckIfUziInHand(rightHandParent);
            if (UziScriptLeftHand == null && UziScriptRightHand != null)
            {
                var distanceToCockBackPosition = Vector3.Distance(UziScriptRightHand.cockBackPosition.position, leftHandModel.transform.position);
                if (distanceToCockBackPosition < 0.1f && !UziScriptRightHand.Cocked)
                {
                    handAnimationController.SetLeftCockBack(true);
                    UziScriptRightHand.CockBack = true;
                    leftHandModel.transform.parent = UziScriptRightHand.cockBackPosition;
                    Invoke("ResetLeftHandModel", 0.5f);
                }
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            var DeagleScriptLeftHand = CheckIfDeagleInHand(leftHandParent);
            var DeagleScriptRightHand = CheckIfDeagleInHand(rightHandParent);
            if (DeagleScriptLeftHand != null && DeagleScriptRightHand == null)
            {
                var distanceToCockBackPosition = Vector3.Distance(DeagleScriptLeftHand.cockBackPosition.position, rightHandModel.transform.position);
                if (distanceToCockBackPosition < 0.1f && !DeagleScriptLeftHand.Cocked)
                {
                    handAnimationController.SetRightCockBack(true);
                    DeagleScriptLeftHand.CockBack = true;
                    rightHandModel.transform.parent = DeagleScriptLeftHand.cockBackPosition;
                    Invoke("ResetRightHandModel", 0.5f);
                }
            }
            var UziScriptLeftHand = CheckIfUziInHand(leftHandParent);
            var UziScriptRightHand = CheckIfUziInHand(rightHandParent);
            if (UziScriptLeftHand != null && UziScriptRightHand == null)
            {
                var distanceToCockBackPosition = Vector3.Distance(UziScriptLeftHand.cockBackPosition.position, rightHandModel.transform.position);
                if (distanceToCockBackPosition < 0.1f && !UziScriptLeftHand.Cocked)
                {
                    handAnimationController.SetRightCockBack(true);
                    UziScriptLeftHand.CockBack = true;
                    rightHandModel.transform.parent = UziScriptLeftHand.cockBackPosition;
                    Invoke("ResetRightHandModel", 0.5f);
                }
            }
        }
    }

    private void Fire()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            var DeagleScript = CheckIfDeagleInHand(rightHandParent);
            var uziScript = CheckIfUziInHand(rightHandParent);
            if (DeagleScript != null)
            {
                DeagleScript.Fire = true;
                handAnimationController.SetRightSqueezeTriggerDeagle();
            }
            else if (uziScript != null)
            {
                handAnimationController.SetRightSqueezeTriggerUzi();
                uziScript.Fire = true;
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            var DeagleScript = CheckIfDeagleInHand(leftHandParent);
            var uziScript = CheckIfUziInHand(leftHandParent);
            if (DeagleScript != null)
            {
                DeagleScript.Fire = true;
                handAnimationController.SetLeftSqueezeTriggerDeagle();
            }
            else if (uziScript != null)
            {
                handAnimationController.SetLeftSqueezeTriggerUzi();
                uziScript.Fire = true;
            }
        }
    }

    private void ReleaseMagazine()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            var deagleScript = CheckIfDeagleInHand(rightHandParent);
            var uziScript = CheckIfUziInHand(rightHandParent);
            if (deagleScript != null && deagleScript.magazine != null)
            {
                deagleScript.ReleaseMagazine = true;
            }
            else if (uziScript != null && uziScript.magazine != null)
            {
                uziScript.ReleaseMagazine = true;
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            var DeagleScript = CheckIfDeagleInHand(leftHandParent);
            var uziScript = CheckIfUziInHand(leftHandParent);
            if (DeagleScript != null)
            {
                DeagleScript.ReleaseMagazine = true;
            }
            else if (uziScript != null && uziScript.magazine != null)
            {
                uziScript.ReleaseMagazine = true;
            }
        }
    }

    private DesertEagle CheckIfDeagleInHand(GameObject handParent)
    {
        var DeagleScript = handParent.GetComponentInChildren<DesertEagle>();
        return DeagleScript;
    }

    private Uzi CheckIfUziInHand(GameObject handParent)
    {
        var UziScript = handParent.GetComponentInChildren<Uzi>();
        return UziScript;
    }

    private void ResetLeftHandModel()
    {
        handAnimationController.SetLeftCockBack(false);
        leftHandModel.transform.parent = leftHandParent.transform;
        leftHandModel.transform.localPosition = Vector3.zero;
        leftHandModel.transform.rotation = Quaternion.Euler(leftHandParent.transform.rotation.eulerAngles + new Vector3(0, 0, 90));
    }

    private void ResetRightHandModel()
    {
        handAnimationController.SetRightCockBack(false);
        rightHandModel.transform.parent = rightHandParent.transform;
        rightHandModel.transform.localPosition = Vector3.zero;
        rightHandModel.transform.rotation = Quaternion.Euler(rightHandParent.transform.rotation.eulerAngles + new Vector3(0, 0, -90));
    }
}
