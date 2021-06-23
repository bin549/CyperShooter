using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;

[RequireComponent(typeof(ItemGrabbable))]
public class Recovery : Item
{
    [SerializeField] private float triggerRange = 0.6f;
    [SerializeField] private Transform target;
    [SerializeField] private float HEALTH_POINT = 5f;
    [SerializeField] private PlayerHealth playerHealth;
    private List<Camera> cameras;
    [SerializeField] private List<Camera> centerEyeAnchorCameras;
    [SerializeField] private AudioClip healSound;
    public ItemGrabbable itemGrabbable;
    public GameObject healEffect;

    private void Awake()
    {
        cameras = FindObjectsOfType<Camera>().ToList();
        centerEyeAnchorCameras = cameras.FindAll(camera => camera.gameObject.name == "CenterEyeAnchor");
        itemGrabbable = this.GetComponent<ItemGrabbable>();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (target == null)
        {
            Perception();
        }
        else
        {
            Trigger();
        }
    }

    private void Perception()
    {
        float shortestDistance = Mathf.Infinity;
        Camera nearestCamera = null;
        foreach (Camera camera in centerEyeAnchorCameras)
        {
            float distanceToCamera = Vector3.Distance(transform.position, camera.transform.position);
            if (distanceToCamera < shortestDistance)
            {
                shortestDistance = distanceToCamera;
                nearestCamera = camera;
            }
        }
        if (nearestCamera != null)
        {
            playerHealth = nearestCamera.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                target = nearestCamera.transform;
            }
        }
    }

    private void Trigger()
    {
        Vector3 dir = target.position - transform.position;
        if (Vector3.Distance(transform.position, target.position) < triggerRange)
        {
        var itemGrabber = transform.parent.GetComponent<ItemGrabber>();

        if (itemGrabber != null)
        {
          itemGrabber.transform.parent.GetComponent<WeaponControllerManager>().canChangeWeapon = true;
          itemGrabber.hand.isFull = false;
          itemGrabber.handVisual.SetActive(true);
        }
            playerHealth.SetHealSound(healSound);
            playerHealth.Heal(HEALTH_POINT);
            PhotonNetwork.Destroy(this.gameObject);
            photonView.RPC("SpawnHealEffect", RpcTarget.All);
        }
    }

    [PunRPC]
    private void SpawnExplosionEffect()
    {
        GameObject effect = Instantiate(healEffect, transform.position, transform.rotation);
        Destroy(effect, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
}
