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
       Debug.Log(runner.ToString());
       if (runner.IsServer)
       {
           if (_cooldownManager.IsCooldownExpired(runner))
           {
               _cooldownManager.ResetCooldown(runner, _config.ShootCooldown); // Reset cooldown timer
               ServerShoot(runner);
               Debug.Log($"Bullet spawned by server for player {runner.LocalPlayer}");
           }
           else
           {
               Debug.Log($"Cooldown still active on server for player {runner.LocalPlayer}");
           }
       }
       /*
       else
       {
           // Clients request the server to shoot
           Debug.Log($"Client requesting to shoot from server: {runner.LocalPlayer}");
           RequestShootRpc(runner);
       }        
       */
    }

    private void ServerShoot(NetworkRunner runner)
    {
        Quaternion bulletRotation = Quaternion.LookRotation(_shootPoint.forward, Vector3.up);
        var bullet = runner.Spawn(_config.BulletPrefab, _shootPoint.position, bulletRotation);
        var bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Shoot(_shootPoint.forward);
    }
    /*
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RequestShootRpc(NetworkRunner runner)
    {
        /*
        if (ShootCooldownTimer.ExpiredOrNotRunning(Runner))
        {
            ShootCooldownTimer = TickTimer.CreateFromSeconds(Runner, _shootCooldown); // Reset cooldown timer
            SpawnBullet(position, direction);
            Debug.Log($"RPC: Bullet spawned by server for player {Runner.LocalPlayer}");
        }
        else
        {
            Debug.Log("RPC: Cooldown still active, no bullet spawned");
        }
        
        Debug.Log(runner.IsServer);
        if (_cooldownManager.IsCooldownExpired(runner))
        {
            _cooldownManager.ResetCooldown(runner, _config.ShootCooldown); // Reset cooldown timer
            ServerShoot(runner);
            Debug.Log($"Bullet spawned by server for player {runner.LocalPlayer}");
        }
    }
    */
}
