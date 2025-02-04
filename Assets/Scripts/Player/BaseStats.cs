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

    [SerializeField, Tooltip("Default value for this stat (read-only at runtime).")]
    private Dictionary<string, float> defaultStats = new Dictionary<string, float>();

    public event Action<string, float, float> OnStatChanged;

    public Action<string, float, float> ExternalStatChangedAction;

    public override void Spawned()
    {
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

            if (ExternalStatChangedAction != null)
                ExternalStatChangedAction += OnExternalStatChanged;
        }
    }

    public void OnDisable()
    {
        if (ExternalStatChangedAction != null)
            ExternalStatChangedAction -= OnExternalStatChanged;
    }

    protected void SetStat(string statName, float newValue)
    {
        if (!Object.HasStateAuthority)
            return;

        float oldValue = Stats[statName];
        bool exists = Stats.TryGet(statName, out oldValue);
        if (!exists)
            return;

        if (!Mathf.Approximately(oldValue, newValue))
        {
            Stats.Set(statName, newValue);
            OnStatChanged?.Invoke(statName, oldValue, newValue);
        }
    }

    protected virtual void OnExternalStatChanged(string statName, float oldValue, float newValue)
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
