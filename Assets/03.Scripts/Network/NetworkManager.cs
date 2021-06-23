using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public GameObject LoginUIPanel;
    public InputField PlayerNameInput;

    [Header("Connecting Info Panel")]
    public GameObject ConnectingInfoUIPanel;
    [Header("GameOptions Panel")]
    public GameObject GameOptionsUIPanel;
    [Header("Creating Room Info Panel")]
    public GameObject CreatingRoomInfoUIPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomUIPanel;
    public InputField RoomNameInputField;
    public string GameMode;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomUIPanel;
    public Text RoomInfoText;
    public Text GameModeText;
    public GameObject PlayerListPrefab;
    public GameObject PlayerListContent;
    public GameObject StartGameButton;
    public Image PanelBackground;
    public Sprite fightModeBackground;
    public Sprite cooperationModeBackground;
    public GameObject[] PlayerSelectionUIGameObjects;
    public FightModePlayer[] FightModePlayers;
    public CooperationModePlayer[] CooperationModePlayers;

    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomUIPanel;
    private Dictionary<int, GameObject> playerListGameObjects;

    public KeyDetector keyDetector;
    public InputField userNameField;
    public InputField roomNameField;
    public GameObject keyBoard;

    [Header("Connection Status")]
    public TextMeshProUGUI connectionStatusText;

    [Header("Wait Panel")]
    public GameObject WaitPanel;

    private void Start()
    {
        ActivatePanel(LoginUIPanel.name);
        PhotonNetwork.AutomaticallySyncScene = true;
        keyDetector.SetTextSource(userNameField);
    }

    private void Update()
    {
        connectionStatusText.text = "Connection status: " + PhotonNetwork.NetworkClientState;

        if (Input.GetKeyDown(KeyCode.P))
        {
            keyDetector.SetSourceText("default player 1");
            OnLoginButtonClicked();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnCreateGameButtonClicked();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            OnJoinGameButtonClicked();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            OnCancelButtonClicked();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameMode = "fight";
            OnCreateRoomButtonClicked();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameMode = "cooperation";
            OnCreateRoomButtonClicked();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            OnStartGameButtonClicked();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnJoinRandomRoomButtonClicked("fight");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnJoinRandomRoomButtonClicked("cooperation");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnLeaveGameButtonClicked();
        }
    }

    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            ActivatePanel(ConnectingInfoUIPanel.name);

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Player name is invalid");
        }
    }

    public override void OnConnected()
    {
        Debug.Log("We connected to internet");
    }

    public override void OnConnectedToMaster()
    {
        keyDetector.SetTextSource(roomNameField);
        keyBoard.SetActive(false);
        ActivatePanel(GameOptionsUIPanel.name);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon.");
    }

    public void OnCreateGameButtonClicked()
    {
        ActivatePanel(CreateRoomUIPanel.name);
    }

    public void OnJoinGameButtonClicked()
    {
        ActivatePanel(JoinRandomRoomUIPanel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        ActivatePanel(CreatingRoomInfoUIPanel.name);

        if (GameMode != null)
        {
            string roomName = RoomNameInputField.text;
            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "Room" + Random.Range(1000, 10000);
            }
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            string[] roomPropsInLobby = { "gm" };
            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", GameMode } };

            roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
            roomOptions.CustomRoomProperties = customRoomProperties;
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(GameOptionsUIPanel.name);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + "Player count:" + PhotonNetwork.CurrentRoom.PlayerCount);

        ActivatePanel(InsideRoomUIPanel.name);

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gm"))
        {
            RoomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
               " Players/Max.Players: " +
               PhotonNetwork.CurrentRoom.PlayerCount + " / " +
               PhotonNetwork.CurrentRoom.MaxPlayers;
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("fight"))
            {
                GameModeText.text = "Fight Mode";
                PanelBackground.sprite = fightModeBackground;

                for (int i = 0; i < PlayerSelectionUIGameObjects.Length; i++)
                {
                    PlayerSelectionUIGameObjects[i].transform.Find("PlayerName").GetComponent<Text>().text = FightModePlayers[i].playerName;
                    PlayerSelectionUIGameObjects[i].GetComponent<Image>().sprite = FightModePlayers[i].playerSprite;
                    PlayerSelectionUIGameObjects[i].transform.Find("PlayerProperty").GetComponent<Text>().text = FightModePlayers[i].weaponName +
                        ": " + "Damage: " + FightModePlayers[i].damage + " FireRate: " + FightModePlayers[i].fireRate;
                }
            }
            else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("cooperation"))
            {
                GameModeText.text = "Cooperation Mode";
                PanelBackground.sprite = cooperationModeBackground;

                for (int i = 0; i < PlayerSelectionUIGameObjects.Length; i++)
                {
                    PlayerSelectionUIGameObjects[i].transform.Find("PlayerName").GetComponent<Text>().text = CooperationModePlayers[i].playerName;
                    PlayerSelectionUIGameObjects[i].GetComponent<Image>().sprite = CooperationModePlayers[i].playerSprite;
                    PlayerSelectionUIGameObjects[i].transform.Find("PlayerProperty").GetComponent<Text>().text = "";
                }
            }

            if (playerListGameObjects == null)
            {
                playerListGameObjects = new Dictionary<int, GameObject>();
            }

            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                GameObject playerListGameObject = GameObject.Instantiate(PlayerListPrefab);
                playerListGameObject.transform.SetParent(PlayerListContent.transform);
                playerListGameObject.transform.localScale = Vector3.one;
                playerListGameObject.transform.localPosition = new Vector3(playerListGameObject.transform.position.x, playerListGameObject.transform.position.y, 0);
                playerListGameObject.GetComponent<PlayerListEntryInitializer>().Initialize(player.ActorNumber, player.NickName);
                object isPlayerReady;
                if (player.CustomProperties.TryGetValue(PlayerStatus.PLAYER_READY, out isPlayerReady))
                {
                    playerListGameObject.GetComponent<PlayerListEntryInitializer>().SetPlayerReady((bool)isPlayerReady);
                }
                playerListGameObjects.Add(player.ActorNumber, playerListGameObject);
            }
        }
        StartGameButton.SetActive(false);
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        GameObject playerListGameObject;
        if (playerListGameObjects.TryGetValue(target.ActorNumber, out playerListGameObject))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue(PlayerStatus.PLAYER_READY, out isPlayerReady))
            {
                playerListGameObject.GetComponent<PlayerListEntryInitializer>().SetPlayerReady((bool)isPlayerReady);
            }
        }
        StartGameButton.SetActive(CheckPlayersReady());
    }

    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (player.CustomProperties.TryGetValue(PlayerStatus.PLAYER_READY, out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void OnStartGameButtonClicked()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gm"))
        {
            ActivatePanel(WaitPanel.name);
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("fight"))
            {
                PhotonNetwork.LoadLevel(SceneNames.FIGHTGAME);
            }
            else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("cooperation"))
            {
                PhotonNetwork.LoadLevel(SceneNames.COOPERATIONGAME);
            }
        }
    }

    public void OnJoinRandomRoomButtonClicked(string _gameMode)
    {
        GameMode = _gameMode;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", _gameMode } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        if (GameMode != null)
        {
            string roomName = RoomNameInputField.text;
            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "Room" + Random.Range(1000, 10000);
            }
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 3;
            string[] roomPropsInLobby = { "gm" };

            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", GameMode } };

            roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
            roomOptions.CustomRoomProperties = customRoomProperties;

            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        RoomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                  " Players/Max.Players: " +
                  PhotonNetwork.CurrentRoom.PlayerCount + " / " +
                  PhotonNetwork.CurrentRoom.MaxPlayers;

        GameObject playerListGameObject = GameObject.Instantiate(PlayerListPrefab);
        playerListGameObject.transform.SetParent(PlayerListContent.transform);
        playerListGameObject.transform.localScale = Vector3.one;
        playerListGameObject.transform.position = Vector3.zero;
        playerListGameObject.GetComponent<PlayerListEntryInitializer>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerListGameObjects.Add(newPlayer.ActorNumber, playerListGameObject);

        StartGameButton.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        RoomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                " Players/Max.Players: " +
                PhotonNetwork.CurrentRoom.PlayerCount + " / " +
                PhotonNetwork.CurrentRoom.MaxPlayers;

        GameObject.Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);
        StartGameButton.SetActive(CheckPlayersReady());
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartGameButton.SetActive(CheckPlayersReady());
        }
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        ActivatePanel(GameOptionsUIPanel.name);
        foreach (GameObject playerListGameobject in playerListGameObjects.Values)
        {
            GameObject.Destroy(playerListGameobject);
        }
        playerListGameObjects.Clear();
        playerListGameObjects = null;
    }

    public void SetGameMode(string _gameMode)
    {
        GameMode = _gameMode;
    }

    public void ActivatePanel(string panelNameToBeActivated)
    {
        LoginUIPanel.SetActive(LoginUIPanel.name.Equals(panelNameToBeActivated));
        ConnectingInfoUIPanel.SetActive(ConnectingInfoUIPanel.name.Equals(panelNameToBeActivated));
        CreatingRoomInfoUIPanel.SetActive(CreatingRoomInfoUIPanel.name.Equals(panelNameToBeActivated));
        CreateRoomUIPanel.SetActive(CreateRoomUIPanel.name.Equals(panelNameToBeActivated));
        GameOptionsUIPanel.SetActive(GameOptionsUIPanel.name.Equals(panelNameToBeActivated));
        JoinRandomRoomUIPanel.SetActive(JoinRandomRoomUIPanel.name.Equals(panelNameToBeActivated));
        InsideRoomUIPanel.SetActive(InsideRoomUIPanel.name.Equals(panelNameToBeActivated));
        WaitPanel.SetActive(WaitPanel.name.Equals(panelNameToBeActivated));
    }
}
