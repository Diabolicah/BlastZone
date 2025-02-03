using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;
using UnityEngine.UIElements;

public class Weapon_Bullet : WeaponDetails
{
    protected Transform _shootPoint;
    protected NetworkPrefabRef _bulletPrefab;
 
    public Weapon_Bullet(BulletWeaponConfig config, Transform shootPoint)
    {
        _bulletPrefab = config.BulletPrefab;
        _shootCooldown = config.ShootCooldown;
        _speed = config.Speed;
        _bulletLifeTime = config.BulletLifeTime;
        _shootPoint = shootPoint;
        _cooldownManager = new CooldownManager();
    }

    protected override void ServerShoot(NetworkRunner runner, PlayerStats playerStats)
    {
        Quaternion bulletRotation = Quaternion.LookRotation(_shootPoint.forward, Vector3.up);
        var bullet = runner.Spawn(_bulletPrefab, _shootPoint.position, bulletRotation);
        var bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Shoot(_shootPoint.forward, _speed, _bulletLifeTime);
    }
}
