using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Triple Bullet Weapon Config")]
public class TripleBulletWeaponConfig : BulletWeaponConfig
{
    [SerializeField] private float _spreadAngle = 15f;

    public float SpreadAngle => _spreadAngle;
}
