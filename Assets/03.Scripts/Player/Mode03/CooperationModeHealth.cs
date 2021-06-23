using UnityEngine;
using Photon.Pun;

public class CooperationModeHealth : MonoBehaviour
{
    public float startHealth = 100;
    [SerializeField] protected float health;
    protected PhotonView photonView;

    protected virtual void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    protected virtual void Start()
    {
        health = startHealth;
    }

    public virtual void TakeDamage(float amount)
    {
        photonView.RPC("TakeDamageRPC", RpcTarget.All, amount);
    }

    [PunRPC]
    public void TakeDamageRPC(float amount)
    {
        health -= amount;
        if (health <= 20.0f)
        {
            Dangerous();
        }
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die() { }

    protected virtual void Dangerous() { }
}
