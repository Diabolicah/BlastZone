using System;
using Fusion;
using UnityEngine;

public class StatsManager : NetworkBehaviour
{
    [Networked]
    public PlayerStatsStruct Stats { get; set; } = PlayerStatsStruct.Default;

    public event Action<PlayerStatsStruct, PlayerStatsStruct> OnStatsChanged;

    private ChangeDetector _changeDetector;
    private PlayerStatsStruct _lastStats;

    public override void Spawned()
    {
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

    public void UpdateHealth(float delta)
    {
        if (Object.HasStateAuthority)
        {
            var newStats = Stats;
            newStats.Health += delta;
            Stats = newStats;
        }
    }
}
