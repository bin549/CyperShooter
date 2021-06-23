using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(ObstacleAudio))]
public class Obstacle : EnvironmentItem
{
    [SerializeField] protected ObstacleAudio obstacleAudio;
    protected PhotonView photonView;

    protected virtual void Awake()
    {
        obstacleAudio = GetComponent<ObstacleAudio>();
        photonView = GetComponent<PhotonView>();
    }

    protected override void Start()
    {
        base.Start();

    }
    protected void OnCollisionEnter(Collision collision)
    {
        obstacleAudio.PlayCollSound();
    }

    public override void TakeDamage()
    { }
}
