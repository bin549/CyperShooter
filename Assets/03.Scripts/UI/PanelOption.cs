using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;

[RequireComponent(typeof(Toggle))]
public class PanelOption : MonoBehaviourPunCallbacks
{
    protected Toggle isSelectedToggle;
    protected GameManager gameManager;
    [SerializeField] protected bool isRetryOption = false;
    [SerializeField] protected bool isMenuOption = false;
    [SerializeField] protected bool isContinueOption = false;
    [SerializeField] protected bool isQuitMatchOption = false;
    [SerializeField] protected bool isResumeOption = false;
    public int currentLevel;

    protected void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        isSelectedToggle = GetComponent<Toggle>();
    }

    public void Init(GamePanel gamePanel)
    {
        isSelectedToggle.group = gamePanel.toggleGroup;
        isSelectedToggle.graphic = transform.GetChild(0).gameObject.GetComponent<Image>();
        UnSelected();
    }

    public void Selected()
    {
        isSelectedToggle.isOn = true;
    }

    public void UnSelected()
    {
        isSelectedToggle.isOn = false;
    }

    public virtual void Confirm()
    {
        string currentLevelName = SceneManager.GetActiveScene().name;
        currentLevel = int.Parse(currentLevelName.Substring(SceneNames.LEVEL.Length, 2));
        if (isRetryOption)
        {
            gameManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (isMenuOption)
        {
            // Destroy(FindObjectOfType<GameManager>());
            gameManager.LoadScene(SceneNames.MENU);
        }
        if (isContinueOption)
        {
            if (currentLevel != PlayerPrefs.GetInt("levelNum"))
            {
                int nextLevel = currentLevel + 1;
                if (currentLevel == PlayerPrefs.GetInt("levelReached"))
                {
                    PlayerPrefs.SetInt("levelReached", nextLevel);
                }
                gameManager.LoadScene(SceneNames.LEVEL + nextLevel.ToString("00"));
            }
            else
            {
                gameManager.LoadScene(SceneNames.LEVEL99);
            }

            //  PlayerPrefs.SetInt("levelReached", levelToUnlock);
            //gameManager.LoadScene(nextLevel);
        }
        if (isQuitMatchOption)
        {
            PhotonNetwork.LeaveRoom();
        }
        if (isResumeOption)
        {
            FindObjectOfType<PlayerUI>().Pause();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(SceneNames.ONLINEMODE);
    }
}
