using UnityEngine;

public class ReadyPoint : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private float readyPointTimer;
    public float isReadyTime = 5.0f;
    [SerializeField] private bool isReady = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.root.gameObject.CompareTag("Player"))
        {
            readyPointTimer += Time.deltaTime;
            if (readyPointTimer > isReadyTime)
            {
                isReady = true;
                animator.SetBool("isReady", true);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.root.gameObject.CompareTag("Player"))
        {
            isReadyTime = 0.0f;
            isReady = false;
        }
    }
}
