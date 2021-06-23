using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class WeaponBox : MonoBehaviour
{
    [SerializeField] private WeaponDB weaponDB;
    [SerializeField] public PhotonView photonView;

    private void Awake()
    {
        weaponDB = FindObjectOfType<WeaponDB>();
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        weaponDB.shootWeaponObtainedIndex = 0;
        var dropoutGrabber = collision.transform.gameObject.GetComponent<DropoutGrabber>();
        if (dropoutGrabber != null)
        {
            Transform dropoutGrabberParent = dropoutGrabber.gameObject.transform.parent;
            WeaponController weaponDropout = weaponDB.GetShootWeapon();
            WeaponControllerManager weaponControllerManager = dropoutGrabberParent.gameObject.GetComponent<WeaponControllerManager>(); ;

            while (weaponDropout != null)
            {
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

              WeaponController weapon = weaponDropout.GetComponent<WeaponController>();
              weapon.Controller = dropoutGrabber.Controller;
              weaponControllerManager.Add(weapon, false);
              weaponDropout = weaponDB.GetShootWeapon();
            }

            weaponControllerManager.Reset();
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
