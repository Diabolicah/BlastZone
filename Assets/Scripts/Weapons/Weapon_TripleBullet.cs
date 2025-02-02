using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;
using UnityEngine.UIElements;

public class Weapon_TripleBullet : IWeapon
{
    private readonly TrippleBulletWeaponConfig _config;
    private readonly Transform _shootPoint;
    private CooldownManager _cooldownManager;

    public Weapon_TripleBullet(TrippleBulletWeaponConfig config, Transform shootPoint)
    {
        _config = config;
        _shootPoint = shootPoint;
        _cooldownManager = new CooldownManager();
    }

    public void fire(NetworkRunner runner)
    {
        if (runner.IsServer)
        {
            if (_cooldownManager.IsCooldownExpired(runner))
            {
                _cooldownManager.ResetCooldown(runner, _config.ShootCooldown); // Reset cooldown timer
                ServerShoot(runner);
            }
        }
    }

    private void ServerShoot(NetworkRunner runner)
    {
        Debug.Log("Shoot Triple");
        Vector3 baseDirection = _shootPoint.forward;
        Vector3 dirRight = Quaternion.Euler(0, _config.SpreadAngle, 0) * baseDirection;
        Vector3 dirLeft = Quaternion.Euler(0, -_config.SpreadAngle, 0) * baseDirection;

        Quaternion bullet1Rotation = Quaternion.LookRotation(baseDirection, Vector3.up);
        Quaternion bullet2Rotation = Quaternion.LookRotation(dirRight);
        Quaternion bullet3Rotation = Quaternion.LookRotation(dirLeft);

        var bullet1 = runner.Spawn(_config.BulletPrefab, _shootPoint.position, bullet1Rotation);
        var bullet2 = runner.Spawn(_config.BulletPrefab, _shootPoint.position, bullet2Rotation);
        var bullet3 = runner.Spawn(_config.BulletPrefab, _shootPoint.position, bullet3Rotation);

        var bullet1Script = bullet1.GetComponent<Bullet>();
        var bullet2Script = bullet2.GetComponent<Bullet>();
        var bullet3Script = bullet3.GetComponent<Bullet>();

        bullet1Script.Shoot(baseDirection);
        bullet2Script.Shoot(dirRight);
        bullet3Script.Shoot(dirLeft);
    }
}
