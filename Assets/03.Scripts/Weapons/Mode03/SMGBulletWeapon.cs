using System;
using UnityEngine;

public class SMGBulletWeapon : BulletWeapon
{
    protected override void Update()
    {
        elapsedTime += Time.deltaTime;
        if (OVRInput.Get(actionButton, Controller) || Input.GetKey(KeyCode.Mouse0))
        {
            if (elapsedTime > fireRate)
            {
                Shoot();
                elapsedTime = 0;
            }
        }
    }
}
