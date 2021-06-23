using UnityEngine;
using Photon.Pun;

public class Destructible : Obstacle
{
    [SerializeField] private Material[] stateMaterials;
    [SerializeField] private MeshRenderer meshRenderer;
    private int stateIndex = 0;

    [SerializeField] private GameObject collEffect;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private Vector3 dir;
    [SerializeField] private bool isDestory;

    protected override void Awake()
    {
        base.Awake();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        meshRenderer.material = stateMaterials[stateIndex];
    }

    public override void TakeDamage()
    {
        Destory();
    }

    public void Destory()
    {
        if (isDestory)
            return;
        stateIndex++;
        if (stateIndex >= stateMaterials.Length)
        {
            photonView.RPC("SpawnDestroyEffect", RpcTarget.All);
            PhotonNetwork.Destroy(gameObject);
            return;
        }
        meshRenderer.material = stateMaterials[stateIndex];
        photonView.RPC("SpawnCollEffectEffect", RpcTarget.All);
    }

    [PunRPC]
    private void SpawnExplosionEffect()
    {
        GameObject.Instantiate(destroyEffect, transform.position, transform.rotation);
        obstacleAudio.PlayDestorySound();
        isDestory = true;
    }

    [PunRPC]
    private void SpawnCollEffectEffect()
    {
        GameObject coll = GameObject.Instantiate(collEffect, transform.position, transform.rotation);
        obstacleAudio.PlayCollSound();
        GameObject.Destroy(coll, coll.GetComponent<ParticleSystem>().duration);
    }
}
