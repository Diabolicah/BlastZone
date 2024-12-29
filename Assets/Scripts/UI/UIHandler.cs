using Fusion;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    private NetworkManager _networkManager;

    private void Start()
    {
        _networkManager = FindFirstObjectByType<NetworkManager>();
    }

    private void OnGUI()
    {
        if (_networkManager != null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                _networkManager.StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                _networkManager.StartGame(GameMode.Client);
            }
        }
    }
}
