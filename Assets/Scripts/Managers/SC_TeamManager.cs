using Fusion;
using UnityEngine;

public class SC_TeamManager : NetworkBehaviour
{
    // Networked team counters, synchronized across all clients.
    [Networked] private int _team1Count { get; set; }
    [Networked] private int _team2Count { get; set; }

    private static SC_TeamManager _instance;
    public static SC_TeamManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            _team1Count = 0;
            _team2Count = 0;
        }
    }

    private void IncrementTeam1Count()
    {
        if (Object.HasStateAuthority)
            _team1Count++;
    }

    private void DecrementTeam1Count()
    {
        if (Object.HasStateAuthority && _team1Count > 0)
            _team1Count--;
    }

    private void IncrementTeam2Count()
    {
        if (Object.HasStateAuthority)
            _team2Count++;
    }

    private void DecrementTeam2Count()
    {
        if (Object.HasStateAuthority && _team2Count > 0)
            _team2Count--;
    }

    public int GetTeam()
    {


        if (_team1Count > _team2Count)
        {
            if (!Object.HasStateAuthority)
            {
                RPC_RequestIncrementTeam(2);
            }
            else
            {
                IncrementTeam2Count();
            }
            return 2;
        }
        else
        {
            if (!Object.HasStateAuthority)
            {
                RPC_RequestIncrementTeam(1);
            }
            else
            {
                IncrementTeam1Count();
            }
            return 1;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestIncrementTeam(int teamNumber, RpcInfo info = default)
    {
        if (!Object.HasStateAuthority)
            return;

        if (teamNumber == 1)
        {
            IncrementTeam1Count();
        }
        else if (teamNumber == 2)
        {
            IncrementTeam2Count();
        }
    }

    public void RemovePlayer(int teamNumber)
    {
        if (!Object.HasStateAuthority)
        {
            RPC_RequestRemovePlayer(teamNumber);
            return;
        }

        if (teamNumber == 1)
        {
            DecrementTeam1Count();
        }
        else if (teamNumber == 2)
        {
            DecrementTeam2Count();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestRemovePlayer(int teamNumber, RpcInfo info = default)
    {
        if (!Object.HasStateAuthority) return;

        if (teamNumber == 1)
        {
            DecrementTeam1Count();
        }
        else if (teamNumber == 2)
        {
            DecrementTeam2Count();
        }
    }

    public int Team1Count => _team1Count;
    public int Team2Count => _team2Count;
}
