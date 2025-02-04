using System;
using Fusion;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;
using Unity.VisualScripting;

public abstract class BaseStats : NetworkBehaviour
{
    [Networked, Capacity(10)]
    public NetworkDictionary<string, float> Stats { get; } = default;

    [SerializeField, Tooltip("Default value for this stat (read-only at runtime). (Limit is 10!!!!)")]
    private Dictionary<string, float> defaultStats = new Dictionary<string, float>();

    public event Action<string, float, float> OnStatChanged;

    [SerializeField]
    protected StatsManager statsManager;

    public override void Spawned()
    {
        if (defaultStats.Count > 10)
        {
            Debug.LogError("Default stats count is more than 10. Please reduce the count to 10 or less.");
            return;
        }

        if (Object.HasStateAuthority)
        {
            foreach (var stat in defaultStats)
            {
                if (!Stats.ContainsKey(stat.Key))
                {
                    Stats.Add(stat.Key, stat.Value);
                }
                else
                {
                    Stats.Set(stat.Key, stat.Value);
                }
            }

            if (statsManager != null)
                statsManager.OnStatsChanged += OnStatManagerChange;
        }
    }

    public void OnDisable()
    {
        if (statsManager != null)
            statsManager.OnStatsChanged -= OnStatManagerChange;
    }

    protected void SetStat(string statName, float newValue)
    {
        if (!Object.HasStateAuthority)
            return;

        float oldValue = GetStat(statName);

        if (!Mathf.Approximately(oldValue, newValue))
        {
            Stats.Set(statName, newValue);
            OnStatChanged?.Invoke(statName, oldValue, newValue);
        }
    }

    protected float GetStat(string statName)
    {
        if (Stats.TryGet(statName, out float value))
        {
            return value;
        }
        throw new KeyNotFoundException($"Stat {statName} not found.");
    }

    protected void ApplyMultiplier(string statName, float multiplier)
    {
        if (!Object.HasStateAuthority)
            return;
        float oldValue = GetStat(statName);
        float newValue = oldValue * multiplier;
        Stats.Set(statName, newValue);
    }

    protected virtual void OnStatManagerChange(PlayerStatsStruct oldPlayerStats, PlayerStatsStruct newPlayerStats)
    {
        throw new NotImplementedException();
    }

    protected void ResetToDefault(string statName)
    {
        if (!Object.HasStateAuthority)
            return;
        if (defaultStats.TryGetValue(statName, out float defaultValue))
        {
            SetStat(statName, defaultValue);
        }
    }

    protected void ResetAllToDefault()
    {
        if (!Object.HasStateAuthority)
            return;
        foreach (var stat in defaultStats)
        {
            SetStat(stat.Key, stat.Value);
        }
    }


}
