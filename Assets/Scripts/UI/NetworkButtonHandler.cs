using Fusion;
using UnityEngine;
using static UI.MainMenu_Logic;
namespace UI
{
    public class NetworkButtonHandler : MonoBehaviour
    {
        private NetworkManager _networkManager; // Reference to the Photon Fusion NetworkManager
        
        private void Start()
        {
            _networkManager = FindFirstObjectByType<NetworkManager>();
        }
        
        public void HostGame()
        {
            if (_networkManager != null)
            {
                _networkManager.StartGame(GameMode.Host);
                unityObjects["Screen_Multiplayer"].gameObject.SetActive(false);
                unityObjects["Img_Background"].gameObject.SetActive(false);
                unityObjects["Screen_Game"].gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("NetworkManager is not assigned!");
            }
        }
        
        public void JoinGame()
        {
            if (_networkManager != null)
            {
                _networkManager.StartGame(GameMode.Client);
                unityObjects["Screen_Multiplayer"].gameObject.SetActive(false);
                unityObjects["Img_Background"].gameObject.SetActive(false);
                unityObjects["Screen_Game"].gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("NetworkManager is not assigned!");
            }
        }
    }
}