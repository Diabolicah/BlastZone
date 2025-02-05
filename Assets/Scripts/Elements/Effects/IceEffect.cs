using Fusion;
using UnityEngine;

public class IceEffect : IElement
{
    private float _slowPercentage;
    private float _slowDuration;
    private CooldownManager _iceSlowManager;
    public IceEffect(float slowPercentage, float slowDuration)
    {
        _slowPercentage = slowPercentage;
        _slowDuration = slowDuration;
        _iceSlowManager = new CooldownManager();
    }

    public void activate(NetworkRunner runner, NetworkObject bulletShooter, Vector3 position, NetworkObject playerHit)
    {
        if (_iceSlowManager.IsCooldownExpiredOrNotRunning(runner))
        {
            MovementSpeed playerMovementSpeed = playerHit.GetComponent<MovementSpeed>();
            if (playerMovementSpeed != null)
            {
                playerMovementSpeed.ApplyTemporaryModifier(_slowPercentage, _slowDuration);
            }
            _iceSlowManager.ResetCooldown(runner, _slowDuration);
        }
    }
}
