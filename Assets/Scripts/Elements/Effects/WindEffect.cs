using Fusion;
using UnityEngine;

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
            Health playerHealth = playerHit.GetComponent<Health>();
            if (playerHealth != null)
            {
                Debug.Log("Wind PushBack Applied");
               
            }
            _windPushBackManager.ResetCooldown(runner, _pushBackDuration);
        }
    }
}
