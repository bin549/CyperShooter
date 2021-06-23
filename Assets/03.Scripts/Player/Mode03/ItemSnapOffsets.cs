using UnityEngine;
using System.Linq;
using Photon.Pun;

public class ItemSnapOffsets : MonoBehaviour
{
    public Transform[] snapOffsets;
    public PhotonView photonView;
    public ItemsManager itemsManager;

    private void Awake()
    {
        itemsManager = FindObjectOfType<ItemsManager>();
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        var transforms = GetComponentsInChildren<Transform>().ToList();
        snapOffsets = transforms.FindAll(tf => tf.gameObject.name.Contains("SnapPosition")).ToArray();

        if (photonView.IsMine)
        {
            itemsManager.SetItemSnapOffsets(this);
        }
    }
}
