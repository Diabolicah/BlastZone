using Fusion;
using UnityEngine;

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
        if (_fireTickManager.IsCooldownExpiredOrNotRunning(runner) && playerHit != null)
        {
            Health playerHealth = playerHit.GetComponent<Health>();
            if (playerHealth != null)
            {
                (bool success, bool isDead) = playerHealth.ApplyDamage(_fireDamage);
                if (success)
                {
                    if (isDead)
                    {
                        LevelingManager LM = bulletShooter.GetComponent<LevelingManager>();
                        DeathXpValue dxv = playerHit.GetComponent<DeathXpValue>();
                        LM.AddExp(dxv.XpValue);
                    }
                }
            }
            _fireTickManager.ResetCooldown(runner, _durationBetweenFireTicks);
        }
    }
}
