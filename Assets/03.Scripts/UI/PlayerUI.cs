using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerUI : MonoBehaviourPunCallbacks
{
    [SerializeField] public LocomotionSetup locomotionSetup;
    [Header("gameCanvas")]
    [SerializeField] public GameObject pausePanel;
    [SerializeField] private GameObject dangerousPanel;
    [SerializeField] private GameObject diePanel;
    [SerializeField] private GameObject waitPanel;
    [SerializeField] private TextMeshProUGUI respawnTimeText;
    [SerializeField] private GameObject HurtPanel;

    private void Start()
    {
        SetPlayerUI();
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("cooperation"))
        {
            locomotionSetup = FindObjectOfType<LocomotionSetup>();
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Z))
        {
            Pause();
        }
    }

    public void Hurt()
    {
        Transform gameCanvas = pausePanel.transform.parent;
        var hurtPanel = Instantiate(HurtPanel, gameCanvas);
        hurtPanel.transform.parent = gameCanvas;
    }

    public void Pause()
    {
        if (pausePanel.gameObject.activeSelf)
        {
            pausePanel.SetActive(false);
            locomotionSetup.EnabledTeleport(true);
        }
        else
        {
            pausePanel.SetActive(true);
            locomotionSetup.EnabledTeleport(false);
        }
    }

    public void SetRespawnTimeText(string respawnTime)
    {
        respawnTimeText.text = respawnTime;
    }

    public void Dangerous(bool isDangerous)
    {
        dangerousPanel.SetActive(isDangerous);
    }

    public void Die(bool isDie)
    {
        diePanel.SetActive(isDie);
    }

    private void SetPlayerUI()
    {


        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (dangerousPanel != null)
        {
            dangerousPanel.SetActive(false);
        }

        if (diePanel != null)
        {
            diePanel.SetActive(false);
        }

        if (waitPanel != null)
        {
            waitPanel.SetActive(false);
        }
    }
}
