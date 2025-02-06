using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        PlayerPrefs.SetInt("Restart", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void BackToMainMenu()
    {
        PlayerPrefs.SetInt("Restart", 1);
        if (PlayerPrefs.HasKey("GameMode"))
            PlayerPrefs.DeleteKey("GameMode");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
