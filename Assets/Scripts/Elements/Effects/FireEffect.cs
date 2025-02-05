using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class FireEffect : IElement
{
    private float _fireDamage;
    private float _fireDamageDuration;
    private float _durationBetweenFireTicks;
    private CooldownManager _fireDurationManager;
    private CooldownManager _fireTickManager;

    public FireEffect(float fireDamage, float fireDamageDuration)
    {
        _fireDamage = fireDamage;
        _fireDamageDuration = fireDamageDuration;
        _durationBetweenFireTicks = 1f;
        _fireDurationManager = new CooldownManager();
        _fireTickManager = new CooldownManager();

    }

    public void activate(NetworkRunner runner, NetworkObject bulletShooter, Vector3 position, NetworkObject playersHit)
    {
        _fireDurationManager.ResetCooldown(runner, _fireDamageDuration);
        while (!_fireDurationManager.IsCooldownExpiredOrNutRunning(runner))
        {
            if(_fireTickManager.IsCooldownExpiredOrNutRunning(runner))
            {
                Debug.Log("Fire Tick");
                _fireTickManager.ResetCooldown(runner, _durationBetweenFireTicks);
            }
        }
    }
}
