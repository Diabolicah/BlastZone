using System.Runtime.Serialization;
using Fusion;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatsUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI HealthText;
    [SerializeField] private TextMeshProUGUI HealthRegenText;
    [SerializeField] private TextMeshProUGUI DamageText;
    [SerializeField] private TextMeshProUGUI AttackSpeedText;
    [SerializeField] private TextMeshProUGUI BulletSpeedText;
    [SerializeField] private TextMeshProUGUI MoveSpeedText;

    [HideInInspector] public StatsManager statsManager;

    public void Activate(StatsManager statsManager)
    {
        this.statsManager = statsManager;
        UpdateStats(statsManager);
        statsManager.OnStatsChanged += (oldStats, newStats) => UpdateStats(statsManager);
    }

    private void UpdateStats(StatsManager statsManager)
    {
        if (statsManager == null)
        {
            return;
        }

        PlayerStatsStruct currentStats = statsManager.Stats;

        HealthText.text = $"Health: x{currentStats.Health}";
        HealthRegenText.text = $"Health Regen: x{currentStats.HealthRegen}";
        DamageText.text = $"Damage: x{currentStats.Damage}";
        AttackSpeedText.text = $"Attack Speed: x{currentStats.AttackSpeed}";
        BulletSpeedText.text = $"Bullet Speed: x{currentStats.BulletSpeed}";
        MoveSpeedText.text = $"Move Speed: x{currentStats.MovementSpeed}";

    }
}