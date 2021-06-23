using UnityEngine;

public class ItemGrabbable : CustomGrabbable
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool isAlive;

    public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        rigidBody.isKinematic = false;
        rigidBody.velocity = linearVelocity;
        rigidBody.angularVelocity = angularVelocity;

        if (isAlive)
        {
            animator.SetBool("BeGrab", false);
            VibrationManager.Instance.TurnOffVibrate(controller);
        }
        GrabEnd();
    }

    public override void GrabBegin(OVRInput.Controller controller)
    {
        base.GrabBegin(controller);
        if (isAlive)
        {
            animator = this.GetComponent<Animator>();
            animator.SetBool("BeGrab", true);
            VibrationManager.Instance.VibrateController(0.1f, 2.9f, controller);
        }
    }
}
 