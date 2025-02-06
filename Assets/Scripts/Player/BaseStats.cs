using System;
using Fusion;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;
using Unity.VisualScripting;
using System.Collections;

public struct TempMultiplier
{
    public float multiplier;
    public float expireTime;
}

public abstract class BaseStats : NetworkBehaviour
{
    [Networked, Capacity(16), SerializeField]
    protected NetworkDictionary<string, float> Stats { get; } = default;

    [Networked, Capacity(16), SerializeField]
    private NetworkDictionary<string, float> defaultStats { get; } = default;
    [Networked, Capacity(16), SerializeField]
    private NetworkDictionary<string, float> baseStats { get; } = default;

    public event Action<string, float, float> OnStatChanged;

    [SerializeField]
    protected StatsManager statsManager;

    private Dictionary<string, float> cachedStats = new Dictionary<string, float>();
    private ChangeDetector _changeDetector;

    private Dictionary<string, List<TempMultiplier>> activeTempMultipliers = new Dictionary<string, List<TempMultiplier>>();


    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        if (Object.HasStateAuthority)
        {
            foreach (var stat in defaultStats)
            {
                if (!Stats.ContainsKey(stat.Key))
                {
                    baseStats.Add(stat.Key, stat.Value);
                    Stats.Add(stat.Key, stat.Value);
                }
                else
                {
                    baseStats.Set(stat.Key, stat.Value);
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
            float newEffectiveValue = Mathf.Max(newValue, 0.1f);
            Stats.Set(statName, newEffectiveValue);
        }
    }

    protected void SetDefaultStat(string statName, float value)
    {
        if (!Object.HasStateAuthority)
            return;
        if (baseStats.ContainsKey(statName))
        {
            baseStats.Set(statName, value);
        }
    }

    protected void ApplyDefaultMultiplierAndReset(string statName, float multiplier)
    {
        if (!Object.HasStateAuthority)
            return;
        if (defaultStats.TryGet(statName, out float defaultValue))
        {
            float newValue = defaultValue * multiplier;
            SetDefaultStat(statName, newValue);
            ResetToDefault(statName);
            if (activeTempMultipliers.TryGetValue(statName, out List<TempMultiplier> multiplierList))
            {
                activeTempMultipliers.Remove(statName);
            }
        }

    }

    public float GetStat(string statName)
    {
        if (Stats.TryGet(statName, out float value))
        {
            return value;
        }
        throw new KeyNotFoundException($"Stat {statName} not found.");
    }

    public void ApplyTemporaryMultiplier(string statName, float multiplier, float duration)
    {
        if (!Object.HasStateAuthority)
            return;

        float currentTime = Runner.SimulationTime;
        float expireTime = currentTime + duration;
        TempMultiplier newEntry = new TempMultiplier { multiplier = multiplier, expireTime = expireTime };

        if (!activeTempMultipliers.TryGetValue(statName, out List<TempMultiplier> multiplierList))
        {
            multiplierList = new List<TempMultiplier>();
            activeTempMultipliers[statName] = multiplierList;
        }
        multiplierList.Add(newEntry);

        float effectiveMultiplier = 1f;
        foreach (var entry in multiplierList)
        {
            effectiveMultiplier += entry.multiplier;
        }

        float baseValue = baseStats.TryGet(statName, out float defVal) ? defVal : GetStat(statName);
        float newEffectiveValue = baseValue * effectiveMultiplier;
        SetStat(statName, newEffectiveValue);

        StartCoroutine(RemoveTempMultiplierAfter(statName, newEntry, duration));
    }

    private IEnumerator RemoveTempMultiplierAfter(string statName, TempMultiplier entry, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!Object.HasStateAuthority)
            yield break;

        if (activeTempMultipliers.TryGetValue(statName, out List<TempMultiplier> multiplierList))
        {
            multiplierList.RemoveAll(e => e.expireTime <= Time.time);

            if (multiplierList.Count == 0)
            {
                activeTempMultipliers.Remove(statName);
            }

            float effectiveMultiplier = 1f;
            foreach (var e in multiplierList)
            {
                effectiveMultiplier += e.multiplier;
            }
            
            float baseValue = baseStats.TryGet(statName, out float defVal) ? defVal : GetStat(statName);
            float newEffectiveValue =  baseValue * effectiveMultiplier;
            SetStat(statName, newEffectiveValue);
        }
    }



    protected virtual void OnStatManagerChange(PlayerStatsStruct oldPlayerStats, PlayerStatsStruct newPlayerStats)
    {
        throw new NotImplementedException();
    }

    protected void ResetToDefault(string statName)
    {
        if (!Object.HasStateAuthority)
            return;
        if (baseStats.TryGet(statName, out float defaultValue))
        {
            SetStat(statName, defaultValue);
        }
    }

    protected void ResetAllToDefault()
    {
        if (!Object.HasStateAuthority)
            return;
        foreach (var stat in baseStats)
        {
            SetStat(stat.Key, stat.Value);
        }
    }


}
