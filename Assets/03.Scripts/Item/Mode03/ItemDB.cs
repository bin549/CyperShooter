using UnityEngine;

public class ItemDB : MonoBehaviour
{
    [SerializeField] private Dropout shootWeaponDropout;
    [SerializeField] private Dropout throwWeaponDropout;
    [SerializeField] private Dropout meleeWeaponDropout;
    [SerializeField] private PowerupHealth powerupHealth;
    [SerializeField] private Gem[] gems;

    public Dropout ShootWeaponDropout { get => shootWeaponDropout; set => shootWeaponDropout = value; }
    public Dropout ThrowWeaponDropout { get => throwWeaponDropout; set => throwWeaponDropout = value; }
    public Dropout MeleeWeaponDropout { get => meleeWeaponDropout; set => meleeWeaponDropout = value; }

    public PowerupHealth PowerupHealth { get => powerupHealth; set => powerupHealth = value; }

    public Gem GetGem(int index)
    {
        return gems[index];
    }
}
