using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class FightModeGameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] PlayerPrefabs;

    [SerializeField] private bool gameStart = false;
    [SerializeField] private bool gameOver = false;

    public bool GameStart { get => gameStart; set => gameStart = value; }
    public bool GameIsOver { get => gameOver; set => gameOver = value; }

    public List<ReadyPoint> readyPoints { get; private set; }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerStatus.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                int randomPosition = Random.Range(-15, 15);
                int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                Vector3 instantiatePosition = readyPoints[actorNumber - 1].transform.position;
                PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name, new Vector3(instantiatePosition.x, 0, instantiatePosition.z), Quaternion.identity);
            }
        }
    }

    public void OnQuitMatchButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(SceneNames.CHOOSE);
    }
}
