using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI PlayerNameText;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject cameraRig;

    private void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("fight"))
        {
            if (photonView.IsMine)
            {
                playerBody.SetActive(false);
            }
            else
            {
                cameraRig.SetActive(false);
                playerBody.SetActive(true);
            }
        }
        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("cooperation"))
        {
            if (photonView.IsMine)
            {
                playerBody.SetActive(false);
            }
            else
            {
                cameraRig.SetActive(false);
                playerBody.SetActive(true);
            }
        }
        SetPlayerUI();
    }

    private void SetPlayerUI()
    {
        if (PlayerNameText != null)
        {
            PlayerNameText.text = photonView.Owner.NickName;

            if (photonView.IsMine)
            {
                PlayerNameText.gameObject.SetActive(false);
            }
        }
    }
}
