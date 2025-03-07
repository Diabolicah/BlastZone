using System;
using Fusion;
using UI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

public class ObjectHealth : Health
{
    public void ResetHealth()
    {
        SetStat(CURRENT_HEALTH, GetStat(MAX_HEALTH));
    }
    protected override void OnDeath()
    {
        GetComponent<RespawnObject>()?.Activate();
    }
}
