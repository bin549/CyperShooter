using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class FightGameCanvas : MonoBehaviour
{
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject waitingPanel;
    public PhotonView photonView;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void LoseLevel()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            losePanel.gameObject.SetActive(false);
            waitingPanel.gameObject.SetActive(true);
        }
    }

    [PunRPC]
    public void Restart()
    {
        PhotonNetwork.LoadLevel(SceneNames.FIGHTGAME);
    }
}
 