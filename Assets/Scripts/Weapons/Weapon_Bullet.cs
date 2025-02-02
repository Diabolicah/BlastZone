using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;
using UnityEngine.UIElements;

public class Weapon_Bullet : IWeapon
{
    private readonly BulletWeaponConfig _config;
    private readonly Transform _shootPoint;
    private CooldownManager _cooldownManager;

    public Weapon_Bullet(BulletWeaponConfig config, Transform shootPoint)
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
        Quaternion bulletRotation = Quaternion.LookRotation(_shootPoint.forward, Vector3.up);
        var bullet = runner.Spawn(_config.BulletPrefab, _shootPoint.position, bulletRotation);
        var bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Shoot(_shootPoint.forward);
    }
}
