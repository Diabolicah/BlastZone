using System;
using Fusion;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;
using Unity.VisualScripting;

public abstract class BaseStats : NetworkBehaviour
{
    [Networked, Capacity(16), SerializeField]
    protected NetworkDictionary<string, float> Stats { get; } = default;

    [Networked, Capacity(16), SerializeField]
    private NetworkDictionary<string, float> defaultStats { get; } = default;

    public event Action<string, float, float> OnStatChanged;

    [SerializeField]
    protected StatsManager statsManager;

    private Dictionary<string, float> cachedStats = new Dictionary<string, float>();
    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

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

    public override void Render()
    {
        foreach (var changedProperty in _changeDetector.DetectChanges(this))
        {
            if (changedProperty == nameof(Stats))
            {
                foreach (var kv in Stats)
                {
                    float cachedValue = 0f;
                    cachedStats.TryGetValue(kv.Key, out cachedValue);
                    float newValue = kv.Value;
                    if (!Mathf.Approximately(cachedValue, newValue))
                    {
                        OnStatChanged?.Invoke(kv.Key, cachedValue, newValue);
                        cachedStats[kv.Key] = newValue;
                    }
                }
            }
        }
    }

    protected virtual void SetStat(string statName, float newValue)
    {
        if (!Object.HasStateAuthority)
            return;

        float oldValue = GetStat(statName);

        if (!Mathf.Approximately(oldValue, newValue))
        {
            Stats.Set(statName, newValue);
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
        if (defaultStats.TryGet(statName, out float defaultValue))
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
