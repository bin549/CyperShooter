using UnityEngine;
using Photon.Pun;

public class Item : MonoBehaviourPunCallbacks
{
    protected ItemsManager itemsManager;

    protected virtual void Start()
    {
        itemsManager = FindObjectOfType<ItemsManager>();

        if (!itemsManager.items.Contains(this))
        {
            itemsManager.items.Add(this);
        }
    }

    public void Clear()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    protected void OnDestroy()
    {
        if (itemsManager)
        {
            itemsManager.items.Remove(this);
        }
    }
}
