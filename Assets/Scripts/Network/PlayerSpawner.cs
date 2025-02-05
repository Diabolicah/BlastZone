using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef playerPrefab;
    [SerializeField] private CardManager CardManager;
    [SerializeField] private StatsUIHandler StatsUIHandler;

    private NetworkObject localPlayerObject;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Vector3 spawnPosition = new Vector3((player.RawEncoded % Runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = Runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
            Runner.SetPlayerObject(player, networkPlayerObject);
            localPlayerObject = networkPlayerObject;
            localPlayerObject.GetComponent<LevelingManager>().cardManager = CardManager;

            StatsUIHandler.Activate(localPlayerObject.GetComponent<StatsManager>());
            localPlayerObject.GetComponent<Player>().Username = SC_LoginLogic.PLayerName;
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
