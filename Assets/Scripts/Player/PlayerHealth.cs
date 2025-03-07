using UI;
using UnityEngine;
using static Unity.Collections.Unicode;

public class PlayerHealth : Health
{
    protected override void OnDeath()
    {
        Player playerComponent = this.GetComponent<Player>();
        if (playerComponent && playerComponent.Team > 0)
        {
            SC_TeamManager.Instance.RemovePlayer(playerComponent.Team);
        }
        int Level = Object.GetComponent<LevelingManager>().Level;
        Runner.Shutdown();
        MainMenu_Logic.unityObjects["Screen_GameOver"].SetActive(true);
    }
}
