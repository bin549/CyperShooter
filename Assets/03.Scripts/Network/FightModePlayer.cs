using UnityEngine;

[CreateAssetMenu(menuName = "Player/FightMode Player")]
public class FightModePlayer : ScriptableObject
{
    public string playerName;
    public Sprite playerSprite;

    [Header("Weapon Properties")]
    public string weaponName;
    public float damage;
    public float fireRate;
    public float bulletSpeed;
}
