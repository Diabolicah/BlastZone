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
        _speed = config.BulletSpeed;
        _bulletLifeTime = config.BulletLifeTime;
        _damage = config.Damage;
        _shootPoint = shootPoint;
        _cooldownManager = new CooldownManager();
    }

    public void fire(NetworkRunner runner, PlayerStatsStruct playerStats, NetworkId bulletShooterId)
    {
        if (_cooldownManager.IsCooldownExpired(runner))
        {
            _cooldownManager.ResetCooldown(runner, _shootCooldown * playerStats.AttackSpeed); // Reset cooldown timer
            ServerShoot(runner, playerStats, runner.FindObject(bulletShooterId));
        }
    }

    protected virtual void ServerShoot(NetworkRunner runner, PlayerStatsStruct playerStats, NetworkObject bulletShooter) {
        Quaternion bulletRotation = Quaternion.LookRotation(_shootPoint.forward, Vector3.up);
        NetworkObject bullet = runner.Spawn(_bulletPrefab, _shootPoint.position, bulletRotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        float speed = _speed * playerStats.BulletSpeed;
        float damage = _damage * playerStats.Damage;
        bulletScript.Init(_shootPoint.position, _shootPoint.forward, speed, _bulletLifeTime, damage, bulletShooter);
    }
}
