using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PowerupHealth : Item
{
    [SerializeField] private Recovery[] recoverys;
    [SerializeField] private GameObject triggerEffectPrefab;
    public PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.root.gameObject.CompareTag("Player"))
        {
            Recovery recovery = PhotonNetwork.Instantiate(recoverys[Random.Range(0, recoverys.Length)].name, transform.position, transform.rotation).GetComponent<Recovery>(); ;
            var snapOffsets = itemsManager.itemSnapOffsets.snapOffsets.ToList();
            recovery.itemGrabbable.snapOffset = snapOffsets.Find(snapOffset => snapOffset.gameObject.name.Contains(recovery.gameObject.name.Split('(')[0]));

            photonView.RPC("SpawnTriggerEffect", RpcTarget.All);

            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    private void SpawnTriggerEffect()
    {
        GameObject triggerEffect = Instantiate(triggerEffectPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(triggerEffect, 1.5f);
    }
}
