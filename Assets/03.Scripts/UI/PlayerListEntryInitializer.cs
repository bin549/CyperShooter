using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerListEntryInitializer : MonoBehaviour
{
    [Header("UI References")]
    public Text PlayerNameText;
    public Button PlayerReadyButton;
    public Image PlayerReadyImage;
    private bool isPlayerReady = false;

    private void Awake()
    {
        PlayerNameText = GameObject.Find("PlayerNameText").GetComponent<Text>();
        PlayerReadyButton = GetComponentInChildren<Button>();
        PlayerReadyImage = GameObject.Find("PlayerReadyImage").GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            PlayerReady();
        }
    }

    public void Initialize(int playerID, string playerName)
    {
        PlayerNameText.text = playerName;
        if (PhotonNetwork.LocalPlayer.ActorNumber != playerID)
        {
            PlayerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable() { { PlayerStatus.PLAYER_READY, isPlayerReady } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);

            PlayerReadyButton.onClick.AddListener(() =>
            {
                PlayerReady();
            });
        }
    }

    public void PlayerReady()
    {
        isPlayerReady = !isPlayerReady;
        SetPlayerReady(isPlayerReady);
        ExitGames.Client.Photon.Hashtable newProps = new ExitGames.Client.Photon.Hashtable() { { PlayerStatus.PLAYER_READY, isPlayerReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(newProps);
    }

    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyImage.enabled = playerReady;

        if (playerReady)
        {
            PlayerReadyButton.GetComponentInChildren<Text>().text = "Ready!";
        }
        else
        {
            PlayerReadyButton.GetComponentInChildren<Text>().text = "Ready?";
        }
    }
}
