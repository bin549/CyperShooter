using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponControllerManager : MonoBehaviour
{
    [SerializeField] private OVRInput.Controller controller;
    [SerializeField] private List<WeaponController> weapons;
    private int currentWeapon = 0;
    [SerializeField] private DropoutGrabber dropoutGrabber;
    [SerializeField] public bool canChangeWeapon = true;

    private void Awake()
    {
        weapons = GetComponentsInChildren<WeaponController>(true).ToList();
        SelectCurrentWeapon(currentWeapon);
    }

    private void Start()
    {
        dropoutGrabber = GetComponentInChildren<DropoutGrabber>();
        dropoutGrabber.Controller = controller;
        dropoutGrabber.ActionButton = OVRInput.Button.PrimaryHandTrigger;
    }

    public void Reset()
    {
        currentWeapon = 0;
        weapons = GetComponentsInChildren<WeaponController>(true).ToList();
        foreach (WeaponController weaponController in weapons)
        {
          weaponController.Controller = controller;
          weaponController.ActionButton = OVRInput.Button.PrimaryIndexTrigger;
        }
        SelectCurrentWeapon(currentWeapon);
    }

    private void Update()
    {
        if (!canChangeWeapon)
            return;
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft, controller) || Input.GetKeyDown(KeyCode.A))
        {
            SelectPreviousWeapon();
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight, controller)|| Input.GetKeyDown(KeyCode.D))
        {
            SelectNextWeapon();
        }
        if (OVRInput.GetDown(OVRInput.Button.Two, controller))
        {
            ShowHand(dropoutGrabber.gameObject);
        }
    }

    public void ShowHand(GameObject hand)
    {
        foreach (WeaponController weaponController in weapons)
        {
            weaponController.gameObject.SetActive(false);
        }
        hand.SetActive(true);
    }

    private void SelectNextWeapon()
    {
        currentWeapon++;
        if (currentWeapon >= weapons.Count)
            currentWeapon = 0;
        weapons[currentWeapon].gameObject.SetActive(true);
        DeactivateOthers();
    }

    private void SelectPreviousWeapon()
    {
        currentWeapon--;
        if (currentWeapon < 0)
            currentWeapon = weapons.Count - 1;
        weapons[currentWeapon].gameObject.SetActive(true);
        DeactivateOthers();
    }

    public void Add(WeaponController weaponController, bool isThrowWeapon = false)
    {
        canChangeWeapon = !isThrowWeapon;
        weapons.Add(weaponController);
        SelectCurrentWeapon(weapons.Count - 1);
    }

    public void Remove(WeaponController weaponController)
    {
        SelectCurrentWeapon(weapons.IndexOf(weaponController) - 1);
        weapons.Remove(weaponController);
    }

    public void IncreaseGunBullet()
    {
        foreach (ShootWeapon shootWeapon in GetComponentsInChildren<ShootWeapon>(true))
        {
            shootWeapon.IncreaseBullet();
        }
    }

    private void DeactivateOthers()
    {
        foreach (WeaponController controller in weapons)
        {
            if (controller != weapons[currentWeapon])
            {
                controller.gameObject.SetActive(false);
            }
        }
    }

    private void SelectCurrentWeapon(int weaponIndex)
    {
        currentWeapon = weaponIndex;

        if (currentWeapon < 0)
            currentWeapon = weapons.Count - 1;
        weapons[currentWeapon].gameObject.SetActive(true);
        DeactivateOthers();
    }
}
