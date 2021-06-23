using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TakeDamage : MonoBehaviourPunCallbacks
{
    public float starthHealth = 100f;
    private float health;
    public Image healthBar;
    private Rigidbody rb;
    public PlayerUI playerUI;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerUI = GameObject.FindObjectOfType<PlayerUI>();
    }

    private void Start()
    {
        health = starthHealth;
        healthBar.fillAmount = health / starthHealth;
    }

    [PunRPC]
    public void DoDamage(float _damage)
    {
        health -= _damage;
        Debug.Log(health);
        healthBar.fillAmount = health / starthHealth;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (photonView.IsMine)
        {
            StartCoroutine(ReSpawn());
        }
    }

    private IEnumerator ReSpawn()
    {
        GameObject canvasGameObject = GameObject.Find("Canvas");

        float respawnTime = 8.0f;
        while (respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1.0f;
        }
        int randomPoint = Random.Range(-20, 20);

        transform.position = new Vector3(randomPoint, 0, randomPoint);
        photonView.RPC("Reborn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Reborn()
    {
        health = starthHealth;
        healthBar.fillAmount = health / starthHealth;
    }
}
