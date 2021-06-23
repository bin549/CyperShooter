using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy[] enemys;
    public Transform[] enemyStartPoints;
    public Transform spawnPoint;
    [HideInInspector] public AudioSource audioSource;
    [SerializeField] private AudioClip spawn_Clip;
    [SerializeField] private WaveSpawner waveSpawner;

    [SerializeField] private bool spawning = false;

    private void Awake()
    {
        waveSpawner = GameObject.FindObjectOfType<WaveSpawner>();
    }

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.clip = spawn_Clip;
    }

    private void Update()
    {
        if (!FindObjectOfType<CooperationModeGameManager>().GameStart)
            return;
        if (spawning)
        {
            return;
        }
        StartCoroutine(SpawnEnemy());
        return;
    }

    private IEnumerator SpawnEnemy()
    {
        spawning = true;
        PlayerStats.Rounds++;
        for (int i = 0; i < enemys.Length; i++)
        {
            Spawn(enemys[i]);
            yield return new WaitForSeconds(1f / enemys.Length * 4);
        }
        if (waveSpawner.enemySpawners.Contains(this))
        {
            waveSpawner.enemySpawners.Remove(this);
        }
        spawning = false;
        gameObject.SetActive(false);
    }

    private void Spawn(Enemy enemy)
    {
        audioSource.Play();
        GameObject newEnemy = PhotonNetwork.Instantiate(enemy.enemyPrefab.name, spawnPoint.position, spawnPoint.rotation) as GameObject;
        EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.startPoint.position = enemyStartPoints[Random.Range(0, enemyStartPoints.Length)].position;
        }
        newEnemy.GetComponent<EnemyDropout>().DropoutType = enemy.dropoutType;
        newEnemy.GetComponent<EnemyDropout>().DropoutPossibility = enemy.dropoutPossibility;
    }
}
