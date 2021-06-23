using UnityEngine;
using Photon.Pun;

public class FightGamePanelOption : PanelOption
{
    public PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public override void Confirm()
    {
        if (isContinueOption)
        {
            OnClickRestart();
        }
    }

    public void OnClickRestart()
    {
        photonView.RPC("Restart", RpcTarget.All);
    }

    [PunRPC]
    public void Restart()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
 