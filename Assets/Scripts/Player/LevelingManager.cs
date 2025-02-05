using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class LevelingManager : NetworkBehaviour
{
    [SerializeField] public int Rank = 1;
    [SerializeField] private int startingLevel = 1;
    [SerializeField] private float startingExp = 0f;
    [SerializeField] public float expToLevelUp = 100f;
    [NonSerialized] public CardManager cardManager;
    [Networked] public int Level { get; set; }
    [Networked] public float Exp { get; set; }

    public event Action<float, float, int> OnStatsChanged;

    private void Start()
    {
        if (Object.HasStateAuthority)
        {
            Level = startingLevel;
            Exp = startingExp;
        }
    }

    public void AddExp(float amount) => RPC_AddExp(amount);

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_AddExp(float amount, RpcInfo info = default)
    {
        Exp += amount;
        while (Exp >= expToLevelUp)
        {
            cardManager?.AddCardSelection(Rank);
            Exp -= expToLevelUp;
            Level++;
        }
        OnStatsChanged?.Invoke(Exp, expToLevelUp, Level);
    }
}
