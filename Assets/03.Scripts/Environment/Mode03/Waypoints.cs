using UnityEngine;
using Photon.Pun;

public class Waypoints : MonoBehaviour
{
    public Transform[] points;
    public int flyTime;
    [SerializeField] private GameObject endEffectPrefab;
    private GameObject endEffect;
    public PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }

    public Transform GetStartPoint()
    {
        return points[0];
    }

    public void End()
    {
        photonView.RPC("SpawnEffect", RpcTarget.All);
    }

    [PunRPC]
    private void SpawnEffect()
    {
        endEffect = Instantiate(endEffectPrefab, points[points.Length - 1].position, points[points.Length - 1].rotation);
        Destroy(endEffect, 0.8f);
    }

    public Transform this[int index]
    {
        get
        {
            return points[index];
        }
    }
}
