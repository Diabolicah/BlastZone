using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LevelingManager : NetworkBehaviour
{
    [SerializeField] public int Rank = 1;
    [SerializeField] private int startingLevel = 1;
    [SerializeField] private float startingExp = 0f;
    [SerializeField] private float expToLevelUp = 100f;
    [Networked] public int Level { get; set; }
    [Networked] public float Exp { get; set; }

    [SerializeField] private List<CardConfig> availableCards;

    public LevelUpUI levelUpUI { get; set; }

    public event Action<List<CardConfig>> OnLevelUp;

    public int pendingLevelUps = 0;

    private void Start()
    {
        if (Object.HasStateAuthority)
        {
            Level = startingLevel;
            Exp = startingExp;
        }
    }

    public void AddExp(float amount)
    {

        if (!Object.HasStateAuthority)
        {
            RPC_AddExp(amount);
            return;
        }
        InternalAddExp(amount);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_AddExp(float amount, RpcInfo info = default)
    {
        InternalAddExp(amount);
    }

    public void InternalAddExp(float amount)
    {
        Exp += amount;
        while (Exp >= expToLevelUp)
        {
            pendingLevelUps++;
            Exp -= expToLevelUp;
            Level++;
        }

        if (pendingLevelUps > 0 && levelUpUI != null && !levelUpUI.IsShowing)
        {
            ShowLevelUpOptionsForNextLevel();
        }
    }

    private void ShowLevelUpOptionsForNextLevel()
    {
        List<CardConfig> cardOptions = GetRandomCardOptions(3);
        Debug.Log($"Leveling up! Level: {Level}, Pending level-ups: {pendingLevelUps}");
        levelUpUI.ShowLevelUpOptions(cardOptions);
        OnLevelUp?.Invoke(cardOptions);
    }


    private List<CardConfig> GetRandomCardOptions(int count)
    {
        
        List<CardConfig> options = new List<CardConfig>();
        while (options.Count < count)
        {
            int index = UnityEngine.Random.Range(0, availableCards.Count);
            if (availableCards[index].GetMinimumRank() <= Rank)
            {
                options.Add(availableCards[index]);
            }
        }
        return options;
    }

    public void OnCardSelected(CardConfig selectedCard)
    {
        ApplyCardEffect(selectedCard);
        pendingLevelUps--;
        if (pendingLevelUps > 0)
        {
            ShowLevelUpOptionsForNextLevel();
        }
    }

    public void ApplyCardEffect(CardConfig selectedCard)
    {
        switch (selectedCard.GetCardType())
        {
            case CardConfig.CardType.Stat:
                StatsManager stats = GetComponent<StatsManager>();
                if (stats != null)
                {
                    PlayerStatsStruct currentStats = stats.Stats;
                    switch (selectedCard.GetTitle())
                    {
                        case "Health":
                            currentStats.Health += 0.1f;
                            break;
                        case "Movement Speed":
                            currentStats.MovementSpeed += 0.1f;
                            break;
                            // Add other stat effects as needed.
                    }
                    stats.UpdateStats(currentStats);
                }
                break;
            case CardConfig.CardType.Ability:
                // Change equipped weapon or ability.
                break;
            case CardConfig.CardType.Element:
                // Change element type or trigger elemental effect.
                break;
            case CardConfig.CardType.Temporary:
                // Apply a temporary buff.
                break;
        }
    }
}
