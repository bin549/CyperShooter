using UnityEngine;
using Photon.Pun;

public class WeaponController : MonoBehaviourPun
{
    [SerializeField] protected OVRInput.Controller controller;
    [SerializeField] protected OVRInput.Button actionButton = OVRInput.Button.PrimaryIndexTrigger;

    public OVRInput.Controller Controller { get => controller; set => controller = value; }
    public OVRInput.Button ActionButton { get => actionButton; set => actionButton = value; }
    protected WeaponAudio weaponAudio;
    protected PhotonView photonView;

    [Range(0, 2.0f)] [SerializeField] protected float duration = 0.15f;
    [Range(0, 4.0f)] [SerializeField] protected float frequency = 0.10f;
    [Range(0, 4.0f)] [SerializeField] protected float amplitude = 1.30f;


    protected virtual void Awake()
    {
        weaponAudio = GetComponent<WeaponAudio>();
        photonView = GetComponent<PhotonView>();
    }

    protected virtual void Update()
    {

    }

}
