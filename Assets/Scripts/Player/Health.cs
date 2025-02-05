using System;
using Fusion;
using UI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

public class Health : BaseStats
{
    private string CURRENT_HEALTH = "CurrentHealth";
    private string MAX_HEALTH = "MaxHealth";
    private string HEALTH_REGEN_RATE = "HealthRegenRate";

    [SerializeField] private GameObject healthBarObject;

    public override void Spawned()
    {
        base.Spawned();
        if (Object.HasStateAuthority)
        {
            SetStat(CURRENT_HEALTH, GetStat(MAX_HEALTH));
        }

        OnStatChanged += UpdateHealthbar;
    }

    private void UpdateHealthbar(string statName, float oldValue, float newValue)
    {
        if (healthBarObject != null && statName == CURRENT_HEALTH)
        {
            float currentHealth = GetStat(CURRENT_HEALTH);
            float maxHealth = GetStat(MAX_HEALTH);
            Image healthBar = healthBarObject.GetComponent<Image>();
            healthBar.fillAmount = currentHealth / maxHealth;
            healthBar.color = Color.Lerp(Color.red, Color.green, currentHealth / maxHealth);
        }
    }

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

    public (bool success, bool isDead) ApplyDamage(float damage)
    {
        if (!Object.HasStateAuthority)
        {
            float currentHealth = GetStat(CURRENT_HEALTH);
            bool isDead = (currentHealth - damage <= 0f);
            RPC_ApplyDamage(damage);
            return (true, isDead);
        }
        else
        {
            return ApplyDamageInternal(damage);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_ApplyDamage(float damage, RpcInfo info = default)
    {
        ApplyDamageInternal(damage);
    }

    private (bool, bool) ApplyDamageInternal(float damage)
    {
        float currentHealth = GetStat(CURRENT_HEALTH);
        float newHealth = Mathf.Max(currentHealth - damage, 0f);
        SetStat(CURRENT_HEALTH, newHealth);

        bool success = true;
        bool isDead = (currentHealth - damage <= 0f);
        if (isDead)
        {   
            DisconnectPlayer();
        }
        return (success, isDead);
    }

    private void DisconnectPlayer()
    {
        int Level = Object.GetComponent<LevelingManager>().Level;
        Runner.Despawn(Object);
        MainMenu_Logic.unityObjects["Screen_GameOver"].SetActive(true);
    }

    public void ApplyHealing(float healing)
    {
        if (!Object.HasStateAuthority)
        {
            RPC_ApplyHealing(healing);
        }
        else
        {
            float currentHealth = GetStat(CURRENT_HEALTH);
            float maxHealth = GetStat(MAX_HEALTH);
            SetStat(CURRENT_HEALTH, Mathf.Min(currentHealth + healing, maxHealth));
        }
    }

    private void ApplyHealingInternal(float healing)
    {
        float currentHealth = GetStat(CURRENT_HEALTH);
        float maxHealth = GetStat(MAX_HEALTH);
        SetStat(CURRENT_HEALTH, Mathf.Min(currentHealth + healing, maxHealth));
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_ApplyHealing(float healing, RpcInfo info = default)
    {
        ApplyHealingInternal(healing);
    }

    protected override void OnStatManagerChange(PlayerStatsStruct oldStats, PlayerStatsStruct newStats)
    {
        if (oldStats.Health != newStats.Health)
        {
            ApplyDefaultMultiplierAndReset(MAX_HEALTH, newStats.Health);
        }

        if (oldStats.HealthRegen != newStats.HealthRegen)
        {
            ApplyDefaultMultiplierAndReset(HEALTH_REGEN_RATE, newStats.HealthRegen);
        }
    }
}
