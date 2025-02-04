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


    private bool _testMovementIncreased = false;

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

        if (Object.HasStateAuthority && !_testMovementIncreased && Runner.SimulationTime > 5f)
        {
            var oldStats = Stats;
            var newStats = Stats;
            newStats.MovementSpeed *= 2f; // Increase by 10%
            Stats = newStats;
            _testMovementIncreased = true;
            // Optionally, you could also invoke the event directly here.
            OnStatsChanged?.Invoke(oldStats, newStats);
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
