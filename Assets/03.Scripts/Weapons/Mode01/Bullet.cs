using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float aliveTime = 5f;

    [SerializeField] private ForceMode bounchForceMode = ForceMode.Force;
    [SerializeField] private ForceMode forceMode = ForceMode.Force;
    [SerializeField] private int playerId;

    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody.AddRelativeForce(transform.forward * speed, forceMode);
        Destroy(gameObject, aliveTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shield") || collision.gameObject.CompareTag("Wall"))
        {
            rigidbody.AddForceAtPosition(transform.position, transform.position, bounchForceMode);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PhotonView>().IsMine)
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, damage);
            }
        }
    }

    public void Initialize(float speed, float damage, int bulletId)
    {
        this.speed = speed;
        this.damage = damage;
        this.playerId = playerId;
    }
}
