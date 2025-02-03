using Fusion;
using UnityEngine;

public class WeaponDetails : IWeapon
{
    protected float _shootCooldown;
    protected float _speed;
    protected float _bulletLifeTime;
    protected CooldownManager _cooldownManager;

    public void fire(NetworkRunner runner, PlayerStats playerStats)
    {
        if (runner.IsServer)
        {
            if (_cooldownManager.IsCooldownExpired(runner))
            {
                _cooldownManager.ResetCooldown(runner, _shootCooldown * ); // Reset cooldown timer
                ServerShoot(runner, playerStats);
            }
        }
    }

    protected virtual void ServerShoot(NetworkRunner runner, PlayerStats playerStats) { }
}
