using Fusion;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WindEffect : IElement
{
    private float _speedBoostPercentage;
    private float _speedBoostDuration;
    private CooldownManager _windspeedBoost;
    public WindEffect(float pushBackForce, float pushBackDuration) {
        _speedBoostPercentage = pushBackForce;
        _speedBoostDuration = pushBackDuration;
        _windspeedBoost = new CooldownManager();

    }
    public void activate(NetworkRunner runner, NetworkObject bulletShooter, Vector3 position, NetworkObject playerHit)
    {
        if (_windspeedBoost.IsCooldownExpiredOrNotRunning(runner))
        {
            MovementSpeed playerMovementSpeed = bulletShooter.GetComponent<MovementSpeed>();
            if (playerMovementSpeed != null)
            {
                playerMovementSpeed.ApplyTemporaryModifier(_speedBoostPercentage, _speedBoostDuration);
            }
            _windspeedBoost.ResetCooldown(runner, _speedBoostDuration);
        }
    }
}
