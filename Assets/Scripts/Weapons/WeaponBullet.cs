using Fusion;
using UnityEngine;

public class WeaponBullet : IWeapon
{
    protected Transform _shootPoint;
    protected NetworkPrefabRef _bulletPrefab;
    protected float _shootCooldown;
    protected float _speed;
    protected float _bulletLifeTime;
    protected float _damage;
    protected CooldownManager _cooldownManager;

    public WeaponBullet(BulletWeaponConfig config, Transform shootPoint)
    {
        _bulletPrefab = config.BulletPrefab;
        _shootCooldown = config.ShootCooldown;
        _speed = config.Speed;
        _bulletLifeTime = config.BulletLifeTime;
        _shootPoint = shootPoint;
        _damage = config.Damage;
        _cooldownManager = new CooldownManager();
    }

    public void fire(NetworkRunner runner, PlayerStats playerStats)
    {
        if (runner.IsServer)
        {
            if (_cooldownManager.IsCooldownExpired(runner))
            {
                _cooldownManager.ResetCooldown(runner, _shootCooldown * playerStats.AttackSpeed); // Reset cooldown timer
                ServerShoot(runner, playerStats);
            }
        }
    }

    protected virtual void ServerShoot(NetworkRunner runner, PlayerStats playerStats) {
        Quaternion bulletRotation = Quaternion.LookRotation(_shootPoint.forward, Vector3.up);
        NetworkObject bullet = runner.Spawn(_bulletPrefab, _shootPoint.position, bulletRotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Shoot(_shootPoint.forward, _speed * playerStats.BulletSpeed, _bulletLifeTime, _damage * playerStats.Damage);
    }
}
