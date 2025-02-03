using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;
using UnityEngine.UIElements;

public class WeaponTripleBullet : WeaponBullet
{
    private float _spreadAngle;

    public WeaponTripleBullet(TripleBulletWeaponConfig config, Transform shootPoint) : base(config, shootPoint)
    {
        _spreadAngle = config.SpreadAngle;
    }

    protected override void ServerShoot(NetworkRunner runner, PlayerStats playerStats)
    {
        Vector3 baseDirection = _shootPoint.forward;
        Vector3 dirRight = Quaternion.Euler(0, _spreadAngle, 0) * baseDirection;
        Vector3 dirLeft = Quaternion.Euler(0, -_spreadAngle, 0) * baseDirection;

        Quaternion bullet1Rotation = Quaternion.LookRotation(baseDirection, Vector3.up);
        Quaternion bullet2Rotation = Quaternion.LookRotation(dirRight);
        Quaternion bullet3Rotation = Quaternion.LookRotation(dirLeft);

        NetworkObject bullet1 = runner.Spawn(_bulletPrefab, _shootPoint.position, bullet1Rotation);
        NetworkObject bullet2 = runner.Spawn(_bulletPrefab, _shootPoint.position, bullet2Rotation);
        NetworkObject bullet3 = runner.Spawn(_bulletPrefab, _shootPoint.position, bullet3Rotation);

        Bullet bullet1Script = bullet1.GetComponent<Bullet>();
        Bullet bullet2Script = bullet2.GetComponent<Bullet>();
        Bullet bullet3Script = bullet3.GetComponent<Bullet>();

        bullet1Script.Shoot(baseDirection, _speed * playerStats.BulletSpeed, _bulletLifeTime, _damage * playerStats.Damage);
        bullet2Script.Shoot(dirRight, _speed * playerStats.BulletSpeed, _bulletLifeTime, _damage * playerStats.Damage);
        bullet3Script.Shoot(dirLeft, _speed * playerStats.BulletSpeed, _bulletLifeTime, _damage * playerStats.Damage);
    }
}
