using System;
using UnityEngine;

[RequireComponent(typeof(WeaponStats))]
public class UpgradeWeaponController : WeaponController
{
    public WeaponStats weaponStats;

    [SerializeField] private Material[] materials;
    protected MeshRenderer meshRenderer;
    private int nextMaterialIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        weaponStats = GetComponent<WeaponStats>();
        meshRenderer = GetComponent<MeshRenderer>();

    }

    protected override void Update()
    {
        base.Update();

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, Controller))
        {
            AddEXP(80);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            AddEXP(80);
        }
    }

    public virtual void UpgradeWeapon()
    {
        Debug.Log(1);
        weaponAudio.PlayUpgradeSound();
        if (materials[nextMaterialIndex] != null)
            meshRenderer.material = materials[nextMaterialIndex];
        nextMaterialIndex++;
    }

    public void AddEXP(int exp_point)
    {
        if (weaponStats.AddEXP(exp_point))
        {
            UpgradeWeapon();
        }
    }

}
