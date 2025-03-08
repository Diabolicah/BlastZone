using UI;
using UnityEngine;
using static Unity.Collections.Unicode;

public class PlayerHealth : Health
{
    protected override void OnDeath()
    {
        base.OnDeath();
        int Level = Object.GetComponent<LevelingManager>().Level;
        Runner.Shutdown();
        MainMenu_Logic.unityObjects["Screen_GameOver"].SetActive(true);
    }
}
