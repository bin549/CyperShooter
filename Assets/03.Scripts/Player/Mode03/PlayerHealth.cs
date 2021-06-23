using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(PlayerAudio))]
public class PlayerHealth : CooperationModeHealth
{
    public GameObject deathEffect;
    public PlayerAudio playerAudio;
    public PlayerUI playerUI;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject body;
    public bool isDangerous;

    [SerializeField] private GameObject healEffectPrefab;
    [SerializeField] private GameObject healEffect;

    protected override void Awake()
    {
        base.Awake();
        playerAudio = GetComponent<PlayerAudio>();
        playerUI = GameObject.FindObjectOfType<PlayerUI>();
        photonView = GetComponent<PhotonView>();
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        playerUI.Hurt();
    }

    public void SetHealSound(AudioClip healSound)
    {
        playerAudio.SetHealSound(healSound);
    }

    protected override void Dangerous()
    {
        playerUI.Dangerous(true);
        isDangerous = true;
    }

    protected override void Die()
    {
        ShowHand(false);
        playerUI.Dangerous(false);
        if (!photonView.IsMine) ShowBody(false);
        playerUI.Die(true);
        if (photonView.IsMine)
        {
            StartCoroutine(ReSpawn());
        }
    }

    private IEnumerator ReSpawn()
    {
        float respawnTime = 5.0f;
        playerUI.SetRespawnTimeText(respawnTime.ToString("0"));
        playerUI.locomotionSetup.EnabledTeleport(false);
        while (respawnTime > 0.0f)
        {
            respawnTime -= 1.0f;
            playerUI.SetRespawnTimeText(respawnTime.ToString("0"));
            yield return new WaitForSeconds(1.0f);
        }
        int randomPosition_x = Random.Range(14, 30);
        int randomPosition_z = Random.Range(-30, -22);
        transform.position = new Vector3(randomPosition_x, 0, randomPosition_z);
        photonView.RPC("Reborn", RpcTarget.AllBuffered);
    }


    [PunRPC]
    public void Reborn()
    {
        health = startHealth;
        ShowHand(true);
        if (!photonView.IsMine) ShowBody(true);
        playerUI.locomotionSetup.EnabledTeleport(true);
        playerUI.Die(false);
    }

    private void ShowHand(bool isShow)
    {
        leftHand.SetActive(isShow);
        rightHand.SetActive(isShow);
    }

    private void ShowBody(bool isShow)
    {
        body.SetActive(isShow);
    }

    public void Heal(float amount)
    {
        photonView.RPC("HealRPC", RpcTarget.All, amount);
        playerAudio.PlayHealSound();
    }

    [PunRPC]
    public void HealRPC(float amount)
    {
        health += amount;

        healEffect = GameObject.Instantiate(healEffectPrefab, transform.root.position, transform.root.rotation);
        GameObject.Destroy(healEffect, 2.0f);

        if (isDangerous)
        {
            if (health > 30.0f)
            {
                isDangerous = false;
                playerUI.Dangerous(isDangerous);

                GameObject effect = Instantiate(healEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1f);

            }
        }

        if (health > startHealth)
            health = startHealth;
    }
    /*
    private IEnumerator Respawn()
    {
        GameObject reSpawnText = GameObject.Find("RespawnText");
        float respawnTime = 8.0f;
        while (respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1.0f;
            transform.GetComponent<PlayerMovementController>().enabled = false;
            reSpawnText.GetComponent<Text>().text = "You are killed. Respawning at: " + respawnTime.ToString(".00");
        }
        animator.SetBool("IsDead", false);
        reSpawnText.GetComponent<Text>().text = "";
        int randomPoint = Random.Range(-20, 20);
        transform.position = new Vector3(randomPoint, 0, randomPoint);
        transform.GetComponent<PlayerMovementController>().enabled = true;
        photonView.RPC("RegainHealth", RpcTarget.AllBuffered);
    }*/

}
