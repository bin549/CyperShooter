using UnityEngine;
using Photon.Pun;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private ObstaclePoints[] obstaclePoints;
    [SerializeField] private int spawnIndex = 0;

    private void Awake()
    {
        obstaclePoints = FindObjectsOfType<ObstaclePoints>();
    }

    public void Spawn()
    {
        foreach (ObstaclePoint obstaclePoint in obstaclePoints[spawnIndex].obstaclePoints)
        {
            PhotonNetwork.Instantiate(obstaclePoint.obstacle.gameObject.name, obstaclePoint.point.position, obstaclePoint.point.rotation);
        }
        spawnIndex++;
    }

}
