using UnityEngine;

public class HandAnimationController : MonoBehaviour
{
    [SerializeField] private Animator leftHandAnimator;
    [SerializeField] private Animator rightHandAnimator;

    public void SetLeftCockBack(bool trueOrFalse)
    {
        leftHandAnimator.SetBool("GrabCockBack", trueOrFalse);
    }

    public void SetRightCockBack(bool trueOrFalse)
    {
        rightHandAnimator.SetBool("GrabCockBack", trueOrFalse);
    }

    public void SetLeftSqueezeTriggerDeagle()
    {
        leftHandAnimator.SetTrigger("TriggerSqueezeDeagle");
    }

    public void SetRightSqueezeTriggerDeagle()
    {
        rightHandAnimator.SetTrigger("TriggerSqueezeDeagle");
    }

    public void SetRightSqueezeTriggerUzi()
    {
        rightHandAnimator.SetBool("SqueezeTriggerUzi", true);
    }

    public void SetRightReleaseTriggerUzi()
    {
        rightHandAnimator.SetBool("SqueezeTriggerUzi", false);
    }

    public void SetLeftSqueezeTriggerUzi()
    {
        leftHandAnimator.SetBool("SqueezeTriggerUzi", true);
    }

    public void SetLeftReleaseTriggerUzi()
    {
        leftHandAnimator.SetBool("SqueezeTriggerUzi", false);
    }

    public void SetHoldingUzi(Animator animator, bool trueOrFalse)
    {
        animator.SetBool("HoldingUzi", trueOrFalse);
    }

    public void SetHoldingUziMagazine(Animator animator, bool trueOrFalse)
    {
        animator.SetBool("HoldMagazine", trueOrFalse);
    }

    public void SetHoldingDeagle(Animator animator, bool trueOrFalse)
    {
        animator.SetBool("HoldingDeagle", trueOrFalse);
    }

    public void SetHoldingDeagleMagazine(Animator animator, bool trueOrFalse)
    {
        animator.SetBool("HoldMagazine", trueOrFalse);
    }
}
