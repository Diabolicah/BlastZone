using System;
using Fusion;
using UnityEngine;

public class StatsManager : NetworkBehaviour
{
    [Networked]
    public PlayerStatsStruct Stats { get; set; }

    public event Action<PlayerStatsStruct> OnStatsChanged;

    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
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
        OnStatsChanged?.Invoke(Stats);
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
