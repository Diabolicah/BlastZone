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

            if (PlayerPrefs.HasKey("Restart"))
            {
                PlayerPrefs.DeleteKey("Restart");
                if (PlayerPrefs.HasKey("GameMode") && !PlayerPrefs.HasKey("IsMainMenu"))
                {
                    unityObjects["Screen_Multiplayer"].gameObject.SetActive(true);
                    unityObjects["Img_Background"].gameObject.SetActive(true);
                    unityObjects["Screen_Login"].gameObject.SetActive(false);
                    ChooseGameMode(PlayerPrefs.GetString("GameMode"));
                }
            }
        }
        
        public async void ChooseGameMode(string gameMode)
        {
            if (_networkManager != null)
            {
                unityObjects["TankColorModel"].SetActive(false);
                unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "";
                DeathmatchButton.GetComponent<Button>().interactable = false;
                TeamDeathmatchButton.GetComponent<Button>().interactable = false;
                DeathmatchText.color = Color.gray;
                loadingText.text = "Loading" + gameMode + " ...";
                bool success = await _networkManager.StartMatchmaking(gameMode);
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
                    loadingText.text = "Failed to connect to " + gameMode;
                    Debug.LogError("Failed to start matchmaking for" +  gameMode);
                }
            }
            else
            {
                Debug.LogError("NetworkManager is not assigned!");
            }
        }
    }
}