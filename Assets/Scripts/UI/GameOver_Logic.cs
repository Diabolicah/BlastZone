using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UI.MainMenu_Logic;

public class GameOver_Logic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private NetworkManager _networkManager; // Reference to the Photon Fusion NetworkManager
    private TextMeshProUGUI RestartloadingText;
    private GameObject RestarthButton;
    private GameObject MainMenuButton;

    private void Start()
    {
        RestartloadingText = GameObject.Find("Txt_RestartLoading").GetComponent<TextMeshProUGUI>();
        RestarthButton = GameObject.Find("Btn_Restart");
        MainMenuButton = GameObject.Find("Btn_MainMenu");
    }

    public async void Restart()
    {
        if (_networkManager != null)
        {
            RestarthButton.GetComponent<Button>().interactable = false;
            MainMenuButton.GetComponent<Button>().interactable = false;
            RestartloadingText.text = "Loading" + NetworkManager.selectedGameMode + " ...";
            bool success = await _networkManager.StartMatchmaking(NetworkManager.selectedGameMode);
            if (success)
            {
                unityObjects["Screen_GameOver"].gameObject.SetActive(false);
                // unityObjects["Screen_Game"].gameObject.SetActive(true);
            }
            else
            {
                RestarthButton.GetComponent<Button>().interactable = true;
                MainMenuButton.GetComponent<Button>().interactable = true;
                RestartloadingText.text = "Failed to Restart " + NetworkManager.selectedGameMode;
                Debug.LogError("Failed to Restart" +  NetworkManager.selectedGameMode);
            }
        }
        else
        {
            Debug.LogError("NetworkManager is not assigned!");
        }
    }
    
    public void BackToMainMenu()
    {
        unityObjects["Screen_GameOver"].gameObject.SetActive(false);
        unityObjects["Screen_MainMenu"].gameObject.SetActive(true);
        unityObjects["Screen_Game"].gameObject.SetActive(false);
        unityObjects["Img_Background"].gameObject.SetActive(true);
    }
}
