using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef playerPrefab;
    [SerializeField] private CardManager CardManager;
    [SerializeField] private StatsUIHandler StatsUIHandler;
    [SerializeField] private LevelUIHandler LevelUIHandler;

    private NetworkObject localPlayerObject;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Vector2 randomPoint = Random.insideUnitCircle * 25f;
            Vector3 spawnPosition = new Vector3(randomPoint.x, 1, randomPoint.y);
            NetworkObject networkPlayerObject = Runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
            Runner.SetPlayerObject(player, networkPlayerObject);
            localPlayerObject = networkPlayerObject;
            localPlayerObject.GetComponent<LevelingManager>().cardManager = CardManager;

            StatsUIHandler.Activate(localPlayerObject.GetComponent<StatsManager>());
            LevelUIHandler.Activate(localPlayerObject.GetComponent<LevelingManager>());

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
