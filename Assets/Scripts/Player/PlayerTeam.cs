using Fusion;
using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerTeam : NetworkBehaviour, IAfterSpawned
{
    // 0 = not assigned, 1 = Team 1, 2 = Team 2, etc.
    [Networked] public int Team { get; set; }
    [SerializeField] private TextMeshProUGUI _PlayerUsername;

    public void AfterSpawned()
    {
        if (Object.HasStateAuthority)
        {
            if (NetworkManager.selectedGameMode == "TeamDeathmatch")
            {
                StartCoroutine(AssignTeamNextFrame());
            }
            else
            {
                Team = 0;
            }
        }
    }

    private IEnumerator AssignTeamNextFrame()
    {
        yield return null;

        AssignTeam();
    }

    private void AssignTeam()
    {
        int team1Count = 0;
        int team2Count = 0;

        PlayerTeam[] allPlayers = FindObjectsByType<PlayerTeam>(FindObjectsSortMode.None);

        foreach (PlayerTeam pScript in allPlayers)
        {
            if (pScript == this)
                continue;

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

        _PlayerUsername.color = Team == 1 ? Color.blue : Team == 2 ? Color.red : Color.white;
    }
}
