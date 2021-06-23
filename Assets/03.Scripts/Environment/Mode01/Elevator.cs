
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private Animator animator;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "player")
        {
            animator.SetTrigger("Rise");
        }
    }
}
