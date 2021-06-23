using UnityEngine;
using UnityEngine.UI;

public class LocomotionSetup : MonoBehaviour
{

    private LocomotionController locomotionController;
    private LocomotionTeleport teleportController
    {
        get
        {
            return locomotionController.GetComponent<LocomotionTeleport>();
        }
    }


    private void Awake()
    {
        locomotionController = FindObjectOfType<LocomotionController>();
    }


    public void EnabledTeleport(bool enabled)
    {
        locomotionController.enabled = enabled;
    }


    public void SetupWalkOnly()
    {
      locomotionController.gameObject.SetActive(false);
    /*    SetupTeleportDefaults();
        teleportController.enabled = false;
        locomotionController.PlayerController.EnableLinearMovement = true;
        locomotionController.PlayerController.RotationEitherThumbstick = false;*/
    }


    public void SetupTeleportOnly()
    {
      locomotionController.gameObject.SetActive(true);
      /*  SetupTeleportDefaults();
        teleportController.enabled = true;
        locomotionController.PlayerController.EnableLinearMovement = true;
        locomotionController.PlayerController.RotationEitherThumbstick = false;*/
    }

    private void SetupTeleportDefaults()
    {
        teleportController.enabled = true;
        locomotionController.PlayerController.RotationEitherThumbstick = false;
        teleportController.EnableMovement(false, false, false, false);
        teleportController.EnableRotation(false, false, false, false);
        var input = teleportController.GetComponent<TeleportInputHandlerTouch>();
        input.InputMode = TeleportInputHandlerTouch.InputModes.CapacitiveButtonForAimAndTeleport;
        input.AimButton = OVRInput.RawButton.A;
        input.TeleportButton = OVRInput.RawButton.A;
        input.CapacitiveAimAndTeleportButton = TeleportInputHandlerTouch.AimCapTouchButtons.A;
        input.FastTeleport = false;
        var hmd = teleportController.GetComponent<TeleportInputHandlerHMD>();
        hmd.AimButton = OVRInput.RawButton.A;
        hmd.TeleportButton = OVRInput.RawButton.A;
        var orient = teleportController.GetComponent<TeleportOrientationHandlerThumbstick>();
        orient.Thumbstick = OVRInput.Controller.LTouch;
    }
}
