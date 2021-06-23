using UnityEngine;
using Photon.Pun;

public enum DropoutType
{
    ShootWeapon,
    ThrowWeapon,
    MeleeWeapon,
};

[RequireComponent(typeof(Animator))]
public class Dropout : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private WeaponController weaponDropout;
    [SerializeField] private DropoutType dropoutType;
    private WeaponDB weaponDB;
    private WeaponController weapon;
    private bool isThrowWeapon = false;
    public PhotonView photonView;
    public DropoutType DropoutType { get => dropoutType; set => dropoutType = value; }

    [SerializeField] private ParticleSystem particleShape;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        weaponDB = FindObjectOfType<WeaponDB>();
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        InitDropoutWeapon();
    }

    public void InitDropoutWeapon()
    {
        switch (dropoutType)
        {
            case DropoutType.ShootWeapon:
                weaponDropout = weaponDB.GetShootWeapon();
                break;
            case DropoutType.ThrowWeapon:
                isThrowWeapon = true;
                weaponDropout = weaponDB.GetThrowWeapon();
                break;
            case DropoutType.MeleeWeapon:
                weaponDropout = weaponDB.GetMeleeWeapon();
                break;
            default:
                weaponDropout = null;
                break;
        }


        if (weaponDropout == null)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        else
        {
            InitParticleShape();
        }
    }

    public void InitParticleShape()
    {
        var shape = particleShape.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Mesh;
        Debug.Log(weaponDropout.gameObject.GetComponent<MeshFilter>().sharedMesh);
        GameObject instance = Instantiate(Resources.Load(weaponDropout.gameObject.name, typeof(GameObject))) as GameObject;
        //shape.mesh = instance.GetComponent<MeshFilter>().sharedMesh;
        Destroy(instance);

        Mesh shapemesh = instance.GetComponent<MeshFilter>().sharedMesh;
        if (shapemesh != null)
        {
            shape.mesh = shapemesh;

        }
        // PhotonNetwork.Instantiate(weaponDropout.gameObject.name);
        //  shape.mesh = Resources.Load(weaponDropout.gameObject.GetComponent<MeshFilter>().sharedMesh.name) as Mesh;
    }

    public void HighLight(bool isLight)
    {
        if (isLight)
            animator.SetTrigger("open");
        else
            animator.SetTrigger("close");
    }

    public void BePickUp(Transform dropoutGrabberParent, OVRInput.Controller controller)
    {
        var cooperationModeGameManager = FindObjectOfType<CooperationModeGameManager>();
        if (!cooperationModeGameManager.HasWeapon)
        {
            cooperationModeGameManager.HasWeapon = true;
            FindObjectOfType<MusicDB>().Play();
        }

        if (weaponDropout != null)
        {
            //        Quaternion weaponDropoutRotation = Quaternion.Euler( instance.transform.localRotation.x + dropoutGrabberParent.transform.rotation.x, instance.transform.localRotation.y + dropoutGrabberParent.transform.rotation.y, instance.transform.localRotation.z + dropoutGrabberParent.transform.rotation.z);
            GameObject weapon_obj = PhotonNetwork.Instantiate(weaponDropout.name, dropoutGrabberParent.position, dropoutGrabberParent.rotation);
            weapon_obj.transform.parent = dropoutGrabberParent;

            WeaponController instance = Instantiate(weaponDropout);
            weapon_obj.transform.localPosition = instance.transform.position;
            weapon_obj.transform.localEulerAngles = new Vector3(
                instance.transform.eulerAngles.x,
                instance.transform.eulerAngles.y,
                instance.transform.eulerAngles.z
            );
            Destroy(instance.gameObject);

            weapon = weapon_obj.GetComponent<WeaponController>();
            weapon.Controller = controller;
            dropoutGrabberParent.GetComponent<WeaponControllerManager>().Add(weapon, isThrowWeapon);
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
            //  dropoutGrabberParent.GetComponent<WeaponControllerManager>().IncreaseGunBullet();
        }
        PhotonNetwork.Destroy(gameObject);
    }
}
