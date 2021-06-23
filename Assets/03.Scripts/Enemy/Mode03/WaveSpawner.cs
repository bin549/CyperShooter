using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

public class WaveSpawner : MonoBehaviour
{
    public CooperationModeGameManager cooperationModeGameManager;
    public Wave[] waves;
    [SerializeField] private int waveIndex = 0;
    public int WaveIndex { get => waveIndex; set => waveIndex = value; }
    public List<EnemySpawner> enemySpawners { get; private set; }
    public ItemDB itemDB;
    public WeatherDB weatherDB;
    public MusicDB musicDB;
    public ItemsManager itemsManager;
    public ObstacleSpawner obstacleSpawner;
    protected EnemyManager enemyManager;
    protected EnvironmentManager environmentManager;


    private void Awake()
    {
        cooperationModeGameManager = FindObjectOfType<CooperationModeGameManager>();
        itemDB = GameObject.FindObjectOfType<ItemDB>();
        enemyManager = FindObjectOfType<EnemyManager>();
        environmentManager = FindObjectOfType<EnvironmentManager>();
        weatherDB = GameObject.FindObjectOfType<WeatherDB>();
        musicDB = GameObject.FindObjectOfType<MusicDB>();
        itemsManager = GameObject.FindObjectOfType<ItemsManager>();
        obstacleSpawner = GameObject.FindObjectOfType<ObstacleSpawner>();
    }

    private void Start()
    {
        enemySpawners = new List<EnemySpawner>();
    }

    private void Update()
    {
        if (!cooperationModeGameManager.GameStart)
            return;

        if (Input.GetKeyDown(KeyCode.N))
        {
            for (int i = 0; i < enemyManager.enemies.Count; i++)
            {
                EnemyHealth e = enemyManager.enemies[i];
                if (e != null)
                {
                    e.TakeDamage(99999.9f);
                }
            }
            for (int i = 0; i < environmentManager.environments.Count; i++)
            {
                EnvironmentItem e = environmentManager.environments[i];
                if (e != null)
                {
                    e.TakeDamage();
                }
            }
        }
        if (enemyManager.enemies.Count > 0)
        {
            return;
        }
        if (enemySpawners.Count != 0)
            return;

        /*  if (!enemySpawners)
          {
            return;
          }*/
        if (WaveIndex == waves.Length)
        {
            if (!cooperationModeGameManager.GameIsOver)
                cooperationModeGameManager.WinLevel();
        }
        else
        {
            {
                WaveIndex++;
                ClearupItem();
                SpawnPowerupHealth();
                NextWave();
                weatherDB.Tomorrow();
                musicDB.Tomorrow();
                obstacleSpawner.Spawn();
            }
        }
    }

    private void ClearupItem()
    {
        foreach (Item item in itemsManager.items)
        {
            item.Clear();
        }
    }

    private void SpawnPowerupHealth()
    {
        int num = Random.Range(1, 5);
        for (int i = 0; i < num; i++)
        {
            int randomPointX = Random.Range(3, 45);
            int randomPointZ = Random.Range(-2, -98);
            PhotonNetwork.Instantiate(itemDB.PowerupHealth.gameObject.name, new Vector3(randomPointX, -3.7f, randomPointZ), Quaternion.identity);
        }
    }

    public void NextWave()
    {
        EnemySpawner[] currentEnemySpawners = waves[waveIndex].enemySpawner;
        foreach (EnemySpawner enemySpawner in currentEnemySpawners)
        {
            enemySpawner.gameObject.SetActive(true);
            if (!enemySpawners.Contains(enemySpawner))
            {
                enemySpawners.Add(enemySpawner);
            }
        }
    }
}
