using System;
using Fusion;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static Unity.Collections.Unicode;

public class Health : BaseStats
{
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            float CurrentHealth = GetStat("CurrentHealth");
            float MaxHealth = GetStat("MaxHealth");
            float HealthRegenRate = GetStat("HealthRegenRate");

            if (CurrentHealth < MaxHealth)
            {
                float newHealth = Mathf.Min(CurrentHealth + HealthRegenRate * Runner.DeltaTime, MaxHealth);
                if (!Mathf.Approximately(newHealth, CurrentHealth))
                {
                    SetStat("CurrentHealth", newHealth);
                }
            }
        }
    }

    public void ApplyDamage(float damage)
    {
        if (!Object.HasStateAuthority)
            return;

        float CurrentHealth = GetStat("CurrentHealth");
        SetStat("CurrentHealth", Mathf.Max(CurrentHealth - damage, 0f));
    }
}
