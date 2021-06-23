using UnityEngine;

[DefaultExecutionOrder(2)]
public class PlayerConstraint : MonoBehaviour
{
    public GameCanvas gameCanvas;
    [SerializeField] protected Transform eyeFollowPos;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private OVRCameraRig cameraRig;

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
        gameManager = FindObjectOfType<GameManager>();
        cameraRig = FindObjectOfType<OVRCameraRig>();
        eyeFollowPos = cameraRig.centerEyeAnchor.GetChild(0);
    }

    private void Start()
    {
        gameCanvas = gameManager.gameCanvas;
        SetupGameCanvas();
    }


    public void SetupGameCanvas()
    {
        gameCanvas.transform.parent = eyeFollowPos;
        gameCanvas.transform.position = eyeFollowPos.position;
        gameCanvas.transform.rotation = eyeFollowPos.rotation;
        gameCanvas.SetupGameCanvas(this);
    }

    public void SetupWalkOnly()
    {
    //  locomotionController.enabled = false;
      locomotionController.gameObject.SetActive(false);

        /*
        SetupTeleportDefaults();
        teleportController.enabled = false;
        locomotionController.PlayerController.EnableLinearMovement = true;
        locomotionController.PlayerController.RotationEitherThumbstick = false;
        */
    }

    public void SetupTeleportOnly()
    {
      //    locomotionController.enabled = true;
        locomotionController.gameObject.SetActive(true);
        /*

        SetupTeleportDefaults();
        teleportController.enabled = true;
        locomotionController.PlayerController.EnableLinearMovement = true;
        locomotionController.PlayerController.RotationEitherThumbstick = false;
        */

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
