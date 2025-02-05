using System.Collections.Generic;
using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;

public class FireEffect : IElement
{
    private float _fireDamage;
    private float _durationBetweenFireTicks;
    private CooldownManager _fireTickManager;

    public FireEffect(float fireDamage)
    {
        _fireDamage = fireDamage;
        _durationBetweenFireTicks = 1f;
        _fireTickManager = new CooldownManager();

    }

    public void activate(NetworkRunner runner, NetworkObject bulletShooter, Vector3 position, NetworkObject playerHit)
    {
        if (_fireTickManager.IsCooldownExpiredOrNotRunning(runner))
        {
            Debug.Log("Fire Tick");
            Health playerHealth = playerHit.GetComponent<Health>();
            if (playerHealth != null)
            {
                Debug.Log("Fire Damage Applied");
                (bool success, bool isDead) = playerHealth.ApplyDamage(_fireDamage);
                if (success)
                {
                    if (isDead)
                    {
                        Debug.Log(bulletShooter.ToString() + " Killed a player");
                    }
                }
            }
            _fireTickManager.ResetCooldown(runner, _durationBetweenFireTicks);
        }
    }
}
