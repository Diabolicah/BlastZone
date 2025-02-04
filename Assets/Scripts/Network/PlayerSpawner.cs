using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef playerPrefab;

    private NetworkObject localPlayerObject;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Vector3 spawnPosition = new Vector3((player.RawEncoded % Runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = Runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
            localPlayerObject = networkPlayerObject;
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (localPlayerObject != null && Runner.LocalPlayer == player)
        {
            Runner.Despawn(localPlayerObject);
            localPlayerObject = null;
        }
    }
}
