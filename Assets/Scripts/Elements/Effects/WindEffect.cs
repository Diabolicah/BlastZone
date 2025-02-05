using Fusion;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WindEffect : IElement
{
    private float _pushBackForce;
    private float _pushBackDuration;
    private CooldownManager _windPushBackManager;
    public WindEffect(float pushBackForce, float pushBackDuration) { 
        _pushBackForce = pushBackForce;
        _pushBackDuration = pushBackDuration;
        _windPushBackManager = new CooldownManager();

    }
    public void activate(NetworkRunner runner, NetworkObject bulletShooter, Vector3 position, NetworkObject playerHit)
    {
        if (_windPushBackManager.IsCooldownExpiredOrNotRunning(runner))
        {
            ApplyExternalForce playerApplyForce = playerHit.GetComponent<ApplyExternalForce>();
            if (playerApplyForce != null)
            {
                Vector3 forceDirection = Vector3.Normalize(playerHit.transform.position - position);
                playerApplyForce.RPC_ApplyForce(forceDirection * _pushBackForce, _pushBackDuration);
            }
            _windPushBackManager.ResetCooldown(runner, _pushBackDuration);
        }
    }
}
