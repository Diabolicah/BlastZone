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
        if (_windPushBackManager.IsCooldownExpiredOrNutRunning(runner))
        {
            Debug.Log("Wind Tick");
            ApplyExternalForce playerApplyForce = playerHit.GetComponent<ApplyExternalForce>();
            if (playerApplyForce != null)
            {
                Debug.Log("Wind PushBack Applied");
                Vector3 forceDirection = Vector3.Normalize(playerHit.transform.position - position);
            }
            _windPushBackManager.ResetCooldown(runner, _pushBackDuration);
        }
    }
}
