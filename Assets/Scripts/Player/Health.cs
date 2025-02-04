using System;
using Fusion;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static Unity.Collections.Unicode;

public class Health : BaseStats
{
    private string CURRENT_HEALTH = "CurrentHealth";
    private string MAX_HEALTH = "MaxHealth";
    private string HEALTH_REGEN_RATE = "HealthRegenRate";
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            float CurrentHealth = GetStat(CURRENT_HEALTH);
            float MaxHealth = GetStat(MAX_HEALTH);
            float HealthRegenRate = GetStat(HEALTH_REGEN_RATE);

            if (CurrentHealth < MaxHealth)
            {
                float newHealth = Mathf.Min(CurrentHealth + HealthRegenRate * Runner.DeltaTime, MaxHealth);
                if (!Mathf.Approximately(newHealth, CurrentHealth))
                {
                    SetStat(CURRENT_HEALTH, newHealth);
                }
            }
        }
    }

    public (bool, bool) ApplyDamage(float damage)
    {
        if (!Object.HasStateAuthority)
            return (false, false);

        float CurrentHealth = GetStat(CURRENT_HEALTH);
        SetStat(CURRENT_HEALTH, Mathf.Max(CurrentHealth - damage, 0f));

        if (CurrentHealth - damage <= 0)
        {
            return (true, true);
        }
        return (true, false);
    }
    public void ApplyHealing(float healing)
    {
        if (!Object.HasStateAuthority)
            return;
        float CurrentHealth = GetStat(CURRENT_HEALTH);
        float MaxHealth = GetStat(MAX_HEALTH);
        SetStat(CURRENT_HEALTH, Mathf.Min(CurrentHealth + healing, MaxHealth));
    }

    protected override void OnStatManagerChange(PlayerStatsStruct oldStats, PlayerStatsStruct newStats)
    {
        if (oldStats.Health != newStats.Health)
        {
            ResetToDefault(CURRENT_HEALTH);
            ApplyMultiplier(CURRENT_HEALTH, newStats.Health);
        }

        if (oldStats.HealthRegen != newStats.HealthRegen)
        {
            ResetToDefault(HEALTH_REGEN_RATE);
            ApplyMultiplier(HEALTH_REGEN_RATE, newStats.HealthRegen);
        }
    }
}
