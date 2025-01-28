using Fusion;
using UnityEngine;

public class CooldownManager
{
    private TickTimer _shootCooldownTimer;

    public bool IsCooldownExpired(NetworkRunner runner){
        return _shootCooldownTimer.ExpiredOrNotRunning(runner);
    }

    public void ResetCooldown(NetworkRunner runner, float duration) {
        _shootCooldownTimer = TickTimer.CreateFromSeconds(runner, duration);
    }
}
