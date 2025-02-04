using System;
using Fusion;
using UnityEngine;

public class StatsManager : NetworkBehaviour
{
    [Networked]
    public PlayerStatsStruct Stats { get; set; }

    public event Action<PlayerStatsStruct, PlayerStatsStruct> OnStatsChanged;

    private ChangeDetector _changeDetector;
    private PlayerStatsStruct _lastStats;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            Stats = PlayerStatsStruct.Default;
        }
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        _lastStats = Stats;
    }

    public override void FixedUpdateNetwork()
    {
        foreach (var changedProperty in _changeDetector.DetectChanges(this))
        {
            if (changedProperty == nameof(Stats))
            {
                HandleStatsChanged();
            }
        }
    }

    // This method gets called when Stats has changed.
    private void HandleStatsChanged()
    {
        var oldStats = _lastStats;
        var newStats = Stats;
        _lastStats = newStats;
        OnStatsChanged?.Invoke(oldStats, newStats);
    }

    public void UpdateStats(PlayerStatsStruct newStats)
    {
        if (!Object.HasStateAuthority)
            return;
        Stats = newStats;
    }
}
