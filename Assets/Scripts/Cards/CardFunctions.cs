using UnityEngine;

public class CardFunctions : MonoBehaviour
{
    public void ActivateCard(CardConfig card, StatsManager statsManager)
    {
        switch (card.GetTitle())
        {
            case "Health":
                ActivateHealthCard(card, statsManager);
                break;
            case "Movement Speed":
                ActivateMovementSpeedCard(card, statsManager);
                break;
        }
    }

    public void ActivateHealthCard(CardConfig card, StatsManager statsManager)
    {
        PlayerStatsStruct currentStats = statsManager.Stats;
        currentStats.Health += 0.1f;
        statsManager.UpdateStats(currentStats);
    }

    public void ActivateMovementSpeedCard(CardConfig card, StatsManager statsManager)
    {
        PlayerStatsStruct currentStats = statsManager.Stats;
        currentStats.MovementSpeed += 0.1f;
        statsManager.UpdateStats(currentStats);
    }
}
