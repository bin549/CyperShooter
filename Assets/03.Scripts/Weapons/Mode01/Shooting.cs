using UnityEngine;
using Photon.Pun;

public class Shooting : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletFrequency = 0.5f;
    [SerializeField] private Transform firePos;
    public FightModePlayer FightModePlayerProperties;

    private float elapsedTime = 0;
    [SerializeField] private int playerId = 0;

    private void Start()
    {
        object playerSelectionNumber;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerStatus.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
        {
            playerId = (int)playerSelectionNumber;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        elapsedTime += Time.deltaTime * 1.0f;
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || Input.GetKey("space"))
        {
            if (elapsedTime >= bulletFrequency)
            {
                photonView.RPC("Fire", RpcTarget.All, firePos.position);
                elapsedTime = 0;
            }
        }
    }

    [PunRPC]
    private void Fire()
    {
        GameObject bullletGameObject = Instantiate(bullet, firePos.position, firePos.rotation);
        bullletGameObject.GetComponent<Bullet>().Initialize(FightModePlayerProperties.bulletSpeed, FightModePlayerProperties.damage, (int)playerId);
    }
}
