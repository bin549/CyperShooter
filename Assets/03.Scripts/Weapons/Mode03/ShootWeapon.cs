using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShootWeapon : UpgradeWeaponController
{
    public Transform firePoint;
    protected float upgradeDamage = 20f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected virtual void Shoot() { }

    public override void UpgradeWeapon()
    {
        base.UpgradeWeapon();
    }

    public virtual void IncreaseBullet()
    {

    }
}
