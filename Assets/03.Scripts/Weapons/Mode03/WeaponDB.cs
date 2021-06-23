using System.Collections.Generic;

public class WeaponDB : Singleton<WeaponDB>
{
    public List<ShootWeapon> shootWeapons;
    public List<ThrowWeapon> throwWeapons;
    public List<MeleeWeapon> meleeWeapons;

    public int shootWeaponObtainedIndex = 0;
    public int throwWeaponObtainedIndex = 0;
    public int meleeWeaponObtainedIndex = 0;

    public int WeaponLevel = 0;

    private void Awake()
    {
        //  DontDestroyOnLoad(this);
    }

    public ShootWeapon GetShootWeapon()
    {
        if (shootWeaponObtainedIndex >= shootWeapons.Count)
        {
            return null;
        }
        return shootWeapons[shootWeaponObtainedIndex++];
    }

    public ThrowWeapon GetThrowWeapon()
    {
        return throwWeapons[UnityEngine.Random.Range(0, throwWeapons.Count)];
    }

    public MeleeWeapon GetMeleeWeapon()
    {
        if (meleeWeaponObtainedIndex >= shootWeapons.Count)
        {
            return null;
        }
        return meleeWeapons[meleeWeaponObtainedIndex++];
    }
}
