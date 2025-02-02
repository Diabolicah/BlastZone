using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Tripple Bullet Weapon Config")]
public class TrippleBulletWeaponConfig : BulletWeaponConfig
{
    [SerializeField] private float _spreadAngle = 15f;

    public float SpreadAngle => _spreadAngle;
}
