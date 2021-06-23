using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAttackTreshold(float attackTreshold)
    {
        animator.SetFloat("attackTreshold", attackTreshold);
    }

    public void Walk(bool walk)
    {
        animator.SetBool(AnimationTags.WALK_PARAMETER, walk);
    }

    public void Run(bool run)
    {
        animator.SetBool(AnimationTags.RUN_PARAMETER, run);
    }

    public void Attack()
    {
        animator.SetFloat("attackTreshold", 0.0f);
        animator.SetTrigger(AnimationTags.ATTACK_TRIGGER);
    }

    public void StrongAttack()
    {
        animator.SetFloat("attackTreshold", 1.0f);
        animator.SetTrigger(AnimationTags.ATTACK_TRIGGER);
    }

    public void Dead()
    {
        animator.SetTrigger(AnimationTags.DEAD_TRIGGER);
    }
}
