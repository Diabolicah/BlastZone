using System;
using Fusion;
using UI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

public class ObjectHealth : Health
{
    protected override void OnDeath()
    {
        Runner.enabled = false;
        //need to active respawn function
    }
}
