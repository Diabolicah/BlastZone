using Fusion;
using UnityEngine;

public class StatsManager : NetworkBehaviour
{
    [Networked]
    public PlayerStatsStruct Stats { get; set; }

    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    // In Render (or FixedUpdateNetwork), detect what properties have changed.
    public override void Render()
    {
        foreach (var changedProperty in _changeDetector.DetectChanges(this))
        {
            // Check if the "Stats" property has changed.
            if (changedProperty == nameof(Stats))
            {
                HandleStatsChanged();
            }
        }

        // Optionally update visual elements—for example, interpolate the material color:
        _material.color = Color.Lerp(_material.color, Color.blue, Time.deltaTime);
    }

    // This method gets called when Stats has changed.
    private void HandleStatsChanged()
    {
        // For example, you might update UI elements or trigger other local effects.
        Debug.Log($"Stats changed: Health is now {Stats.Health}");
    }
}
