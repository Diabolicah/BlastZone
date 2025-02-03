using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;
using UnityEngine.UIElements;

public class Weapon_TripleBullet : WeaponDetails
{
    private readonly Transform _shootPoint;
    private readonly NetworkPrefabRef _bulletPrefab;
    private float _spreadAngle;
 

    public Weapon_TripleBullet(TripleBulletWeaponConfig config, Transform shootPoint)
    {
        _bulletPrefab = config.BulletPrefab;
        _shootCooldown = config.ShootCooldown;
        _spreadAngle = config.SpreadAngle;
        _speed = config.Speed;
        _bulletLifeTime = config.BulletLifeTime;
        _shootPoint = shootPoint;
        _cooldownManager = new CooldownManager();
    }

    protected override void ServerShoot(NetworkRunner runner, PlayerStats playerStats)
    {
        Vector3 baseDirection = _shootPoint.forward;
        Vector3 dirRight = Quaternion.Euler(0, _spreadAngle, 0) * baseDirection;
        Vector3 dirLeft = Quaternion.Euler(0, -_spreadAngle, 0) * baseDirection;

        Quaternion bullet1Rotation = Quaternion.LookRotation(baseDirection, Vector3.up);
        Quaternion bullet2Rotation = Quaternion.LookRotation(dirRight);
        Quaternion bullet3Rotation = Quaternion.LookRotation(dirLeft);

        var bullet1 = runner.Spawn(_bulletPrefab, _shootPoint.position, bullet1Rotation);
        var bullet2 = runner.Spawn(_bulletPrefab, _shootPoint.position, bullet2Rotation);
        var bullet3 = runner.Spawn(_bulletPrefab, _shootPoint.position, bullet3Rotation);

        var bullet1Script = bullet1.GetComponent<Bullet>();
        var bullet2Script = bullet2.GetComponent<Bullet>();
        var bullet3Script = bullet3.GetComponent<Bullet>();

        bullet1Script.Shoot(baseDirection, _speed, _bulletLifeTime);
        bullet2Script.Shoot(dirRight, _speed, _bulletLifeTime);
        bullet3Script.Shoot(dirLeft, _speed, _bulletLifeTime);
    }
}
