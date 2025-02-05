using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UI.MainMenu_Logic;
namespace UI
{
    public class NetworkButtonHandler : MonoBehaviour
    {
        private NetworkManager _networkManager; // Reference to the Photon Fusion NetworkManager
        private TextMeshProUGUI loadingText;
        private GameObject DeathmatchButton;
        private TextMeshProUGUI DeathmatchText;
        private GameObject TeamDeathmatchButton;
        private TextMeshProUGUI TeamDeathmatchText;

        private void Start()
        {
            loadingText = GameObject.Find("Txt_Loading").GetComponent<TextMeshProUGUI>();
            DeathmatchButton = GameObject.Find("Btn_Deathmatch");
            TeamDeathmatchButton = GameObject.Find("Btn_TeamDeathmatch");
            DeathmatchText = DeathmatchButton.GetComponentInChildren<TextMeshProUGUI>();
            TeamDeathmatchText = TeamDeathmatchButton.GetComponentInChildren<TextMeshProUGUI>();
            _networkManager = FindFirstObjectByType<NetworkManager>();
        }
        
        public async void PlayDeathmatch()
        {
            if (_networkManager != null)
            {
                DeathmatchButton.GetComponent<Button>().interactable = false;
                TeamDeathmatchButton.GetComponent<Button>().interactable = false;
                DeathmatchText.color = Color.gray;
                loadingText.text = "Loading Deathmatch ...";
                bool success = await _networkManager.StartMatchmaking("Deathmatch");
                if (success)
                {
                    unityObjects["Screen_Multiplayer"].gameObject.SetActive(false);
                    unityObjects["Img_Background"].gameObject.SetActive(false);
                    unityObjects["Screen_Game"].gameObject.SetActive(true);
                }
                else
                {
                    DeathmatchButton.GetComponent<Button>().interactable = true;
                    TeamDeathmatchButton.GetComponent<Button>().interactable = true;
                    DeathmatchText.color = Color.white;
                    loadingText.text = "Failed to connect to deathmatch";
                    Debug.LogError("Failed to start matchmaking for Deathmatch");
                }
            }
            else
            {
                Debug.LogError("NetworkManager is not assigned!");
            }
        }
        
        public async void PlayTeamDeathmatch()
        {
            if (_networkManager != null)
            {
                DeathmatchButton.GetComponent<Button>().interactable = false;
                TeamDeathmatchButton.GetComponent<Button>().interactable = false;
                TeamDeathmatchText.color = Color.gray;
                loadingText.text = "Loading Team Deathmatch ...";
                bool success = await _networkManager.StartMatchmaking("TeamDeathmatch");
                if(success)
                {
                    unityObjects["Screen_Multiplayer"].gameObject.SetActive(false);
                    unityObjects["Img_Background"].gameObject.SetActive(false);
                    unityObjects["Screen_Game"].gameObject.SetActive(true);
                }
                else
                {
                    DeathmatchButton.GetComponent<Button>().interactable = true;
                    TeamDeathmatchButton.GetComponent<Button>().interactable = true;
                    TeamDeathmatchText.color = Color.white;
                    loadingText.text = "Failed to connect to team deathmatch";
                    Debug.LogError("Failed to start matchmaking for Team Deathmatch");
                }
            }
            else
            {
                Debug.LogError("NetworkManager is not assigned!");
            }
        }
    }
}