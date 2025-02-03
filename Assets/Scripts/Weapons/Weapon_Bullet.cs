using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;
using UnityEngine.UIElements;

public class Weapon_Bullet : WeaponBullet
{
    public Weapon_Bullet(BulletWeaponConfig config, Transform shootPoint) : base(config, shootPoint) { }
}
