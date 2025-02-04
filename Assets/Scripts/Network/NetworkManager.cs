using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    private NetworkRunner _runner;

    [SerializeField] private string selectedGameMode = "Deathmatch";

    public async void StartMatchmaking(string gameMode)
    {
        selectedGameMode = gameMode;

        // Create the Fusion runner if not already created.
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        StartGameArgs args = new StartGameArgs
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = selectedGameMode,
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        };

        var result = await _runner.StartGame(args);

        if (!result.Ok)
        {
            Debug.LogError($"Failed to start game: {result.ShutdownReason}");
        }
        else
        {
            Debug.Log($"Matchmaking started successfully for gamemode: {selectedGameMode}");
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        InputHandler.CollectInput(input);
    }

    // Other INetworkRunnerCallbacks methods remain empty
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}
