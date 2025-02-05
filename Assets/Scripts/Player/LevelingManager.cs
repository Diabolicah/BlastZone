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

    private LevelUpUI levelUpUI;

    public event Action<List<CardConfig>> OnLevelUp;

    private void Start()
    {
        if (Object.HasStateAuthority)
        {
            Level = startingLevel;
            Exp = startingExp;

            levelUpUI = FindFirstObjectByType<LevelUpUI>();
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
        if (Exp >= expToLevelUp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        Exp -= expToLevelUp;
        List<CardConfig> cardOptions = GetRandomCardOptions(3);
        levelUpUI?.ShowLevelUpOptions(cardOptions);
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
                    }
                    stats.UpdateStats(currentStats);
                }
                break;

            case CardConfig.CardType.Ability:
                // Change equipped weapon or ability
                // Your custom logic here.
                break;

            case CardConfig.CardType.Element:
                // Change element type or trigger elemental effect
                break;

            case CardConfig.CardType.Temporary:
                // Apply a temporary buff
                break;
        }
    }
}
