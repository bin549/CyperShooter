using UnityEngine;
using Photon.Pun;

public class PlayerSelection : MonoBehaviour
{
    public GameObject[] selectablePlayers;

    public int playerSelectionNumber;

    private void Start()
    {
        playerSelectionNumber = 0;
        ActivatePlayer(playerSelectionNumber);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PreviousPlayer();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            NextPlayer();
        }
    }

    private void ActivatePlayer(int x)
    {
        foreach (GameObject selectablePlayer in selectablePlayers)
        {
            selectablePlayer.SetActive(false);
        }
        selectablePlayers[x].SetActive(true);
        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable() { { PlayerStatus.PLAYER_SELECTION_NUMBER, playerSelectionNumber } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }

    public void NextPlayer()
    {
        playerSelectionNumber += 1;
        if (playerSelectionNumber >= selectablePlayers.Length)
        {
            playerSelectionNumber = 0;
        }
        ActivatePlayer(playerSelectionNumber);
    }

    public void PreviousPlayer()
    {
        playerSelectionNumber -= 1;
        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = selectablePlayers.Length - 1;
        }
        ActivatePlayer(playerSelectionNumber);
    }
}
