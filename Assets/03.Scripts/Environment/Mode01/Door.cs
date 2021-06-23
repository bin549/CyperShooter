using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    public string menuSceneName = "MainMenu";
    public string nextLevel = "Level02";
    public int levelToUnlock = 2;

    public OVROverlay button;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Key") && !isOpen)
        {
            animator.Play("open");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Key") && isOpen)
        {
            Invoke("Close", 3.0f);
        }
    }

    private void Close()
    {
        animator.Play("close");
    }

    private void LevelFinish()
    {
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
    }
}