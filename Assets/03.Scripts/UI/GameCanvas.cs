using UnityEngine;

[DefaultExecutionOrder(1)]
public class GameCanvas : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    public PlayerConstraint playerConstraint;

    [SerializeField] private GameObject DangerousPanel;
    [SerializeField] private GameObject HurtPanel;

    [SerializeField] private GameObject winGuardianEffect;

    [SerializeField] private GameObject winEffectPrefab;
    [SerializeField] private GameObject winEffect;

    [SerializeField] private GameObject loseEffectPrefab;
    [SerializeField] private GameObject loseEffect;
    [SerializeField] private GameObject loseGuardianEffect;

    private Animator loadAnimator;

    public Animator LoadAnimator()
    {
        return loadAnimator;
    }

    public void Awake()
    {
        pausePanel = GameObject.Find("PausePanel");
        winPanel = GameObject.Find("WinPanel");
        losePanel = GameObject.Find("LosePanel");
        FindObjectOfType<GameManager>().gameCanvas = this;
        loadAnimator = winGuardianEffect.GetComponent<Animator>();
    }

    public void Start()
    {
        HidePanel();

        loadAnimator.SetTrigger("fade_out");
    }

    public void Dangerous()
    {
        DangerousPanel.SetActive(true);
    }

    public void Hurt()
    {
        var hurtPanel = Instantiate(HurtPanel, transform);
        hurtPanel.transform.parent = transform;
    }

    public void SetupGameCanvas(PlayerConstraint playerConstraint)
    {
        this.playerConstraint = playerConstraint;
    }

    public void HidePanel()
    {
        pausePanel.gameObject.SetActive(false);
        winPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
        DangerousPanel.gameObject.SetActive(false);
    }

    public void WinLevel()
    {
        winEffect = GameObject.Instantiate(winEffectPrefab, transform.root.position, transform.root.rotation);
        GameObject.Destroy(winEffect, 2.0f);
        winGuardianEffect.SetActive(true);
        winPanel.gameObject.SetActive(true);
        playerConstraint.SetupWalkOnly();
    }

    public void LoseLevel()
    {
        loseEffect = GameObject.Instantiate(loseEffectPrefab, transform.root.position, transform.root.rotation);
        GameObject.Destroy(loseEffect, 2.0f);
        loseGuardianEffect.SetActive(true);
        losePanel.gameObject.SetActive(true);
        playerConstraint.SetupWalkOnly();
    }

    public void Pause(bool isPause)
    {
        if (!isPause)
        {
            pausePanel.gameObject.SetActive(false);
            playerConstraint.SetupTeleportOnly();
        }
        else
        {
            pausePanel.gameObject.SetActive(true);
            playerConstraint.SetupWalkOnly();
        }
    }
}
