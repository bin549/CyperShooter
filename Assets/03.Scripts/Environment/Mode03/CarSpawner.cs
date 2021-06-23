using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CarSpawner : MonoBehaviour
{
    private CarDB carDB;
    [SerializeField] private Waypoints[] waypoints;
    public GameObject spawnerEffect;
    public float timeBetweenSpawns = 5f;
    [SerializeField] private float spawn_Timer;
    public PhotonView photonView;

    private void Awake()
    {
        carDB = GetComponent<CarDB>();
        waypoints = FindObjectsOfType<Waypoints>();
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        spawn_Timer += Time.deltaTime;
        if (spawn_Timer > timeBetweenSpawns)
        {
            var waypoint = waypoints[Random.Range(0, waypoints.Length)];
            PhotonNetwork.Instantiate(carDB.cars[Random.Range(0, carDB.cars.Length)].car.name, waypoint[0].position, waypoint[0].rotation);
            spawn_Timer = 0f;
        }
    }
}
