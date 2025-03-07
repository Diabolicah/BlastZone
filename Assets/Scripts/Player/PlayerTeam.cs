using Fusion;
using UnityEngine;

public class PlayerTeam : NetworkBehaviour
{
    // 0 = not assigned, 1 = Team 1, 2 = Team 2, etc.
    [Networked] public int Team { get; set; }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            if (NetworkManager.selectedGameMode == "TeamDeathmatch")
            {
                AssignTeam();
            }
            else
            {
                Team = 0;
            }
        }
    }

    private void AssignTeam()
    {
        int team1Count = 0;
        int team2Count = 0;

        PlayerTeam[] allPlayers = FindObjectsByType<PlayerTeam>(FindObjectsSortMode.None);

        foreach (PlayerTeam pScript in allPlayers)
        {
             if (pScript == this) continue;

            if (pScript.Team == 1) team1Count++;
            if (pScript.Team == 2) team2Count++;
        }

        if (team1Count > team2Count)
        {
            Team = 2;
        }
        else
        {
            Team = 1;
        }
    }
}
