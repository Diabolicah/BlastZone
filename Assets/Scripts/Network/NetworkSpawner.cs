using Fusion;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class NetworkSpawner : MonoBehaviour
{
    [SerializeField] private NetworkPrefabRef playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private CinemachineCamera virtualCamera;

    private void Start()
    {
        virtualCamera = FindFirstObjectByType<CinemachineCamera>();
        if (virtualCamera == null)
        {
            Debug.LogWarning("CinemachineCamera not found in the scene.");
        }
    }

    public void HandlePlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // Spawn the player object on the server
        if (runner.IsServer)
        {
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
            spawnedCharacters.Add(player, networkPlayerObject);
        }

        // Ensure the local player's camera follows their own object
        if (runner.LocalPlayer == player && virtualCamera != null)
        {
            // Find the local player's NetworkObject
            foreach (var kvp in spawnedCharacters)
            {
                if (kvp.Key == player)
                {
                    virtualCamera.Follow = kvp.Value.transform;
                    break;
                }
            }
        }
    }


    public void HandlePlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            spawnedCharacters.Remove(player);
        }
    }
}
