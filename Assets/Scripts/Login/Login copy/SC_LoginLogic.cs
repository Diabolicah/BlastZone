using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class SC_LoginLogic : MonoBehaviour
{
    // public Dictionary<string, GameObject> unityObjects;
    private string UserName = string.Empty;
    private string userId = string.Empty;
    private string tempMatchId = "";

    #region Singleton
    static SC_LoginLogic instance;
    public static SC_LoginLogic Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("SC_LoginLogic").GetComponent<SC_LoginLogic>();

            return instance;
        }
    }
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        SC_LoginServerApi.OnGeneralResponse += OnGeneralResponse;
        // SC_WebSocket.OnConnect += OnConnect;
        // SC_WebSocket.OnDisconnect += OnDisconnect;
        // SC_WebSocket.OnErrorReceived += OnDisconnect;
        // SC_WebSocket.OnCancelMatching += OnCancelMatching;
        // SC_WebSocket.OnSendMessage += OnSendMessage;
        // SC_WebSocket.OnBroadcastMessage += OnBroadcastMessage;
        // SC_WebSocket.OnReadyToPlay += OnReadyToPlay;
        // SC_WebSocket.OnStartGame += OnStartGame;
        // SC_WebSocket.OnSendMove += OnSendMove; 
    }

    private void OnDisable()
    {
        SC_LoginServerApi.OnGeneralResponse -= OnGeneralResponse;
        // SC_WebSocket.OnConnect -= OnConnect;
        // SC_WebSocket.OnDisconnect -= OnDisconnect;
        // SC_WebSocket.OnErrorReceived -= OnDisconnect;
        // SC_WebSocket.OnCancelMatching -= OnCancelMatching;
        // SC_WebSocket.OnSendMessage -= OnSendMessage;
        // SC_WebSocket.OnBroadcastMessage -= OnBroadcastMessage;
        // SC_WebSocket.OnReadyToPlay -= OnReadyToPlay;
        // SC_WebSocket.OnStartGame -= OnStartGame;
        // SC_WebSocket.OnSendMove -= OnSendMove;
    }

    private void Start()
    {
        MainMenu_Logic.unityObjects = new Dictionary<string, GameObject>();
        GameObject[] _objects = GameObject.FindGameObjectsWithTag("UnityObject");
        foreach (GameObject g in _objects)
            MainMenu_Logic.unityObjects.Add(g.name, g);
        
        MainMenu_Logic.unityObjects["Screen_Register"].SetActive(false);
        // unityObjects["Screen_Lobby"].SetActive(false);
        //unityObjects["Screen_SearchingOpponent"].SetActive(false);
        //unityObjects["Game"].SetActive(false);
    }
    #endregion


    #region Controller
    public void Btn_Login_Login()
    {
        Debug.Log("Btn_Login_Login");
        MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;

        UserName =  MainMenu_Logic.unityObjects["InputField_Login_Username"].GetComponent<TMP_InputField>().text;
        string _password =  MainMenu_Logic.unityObjects["InputField_Login_Password"].GetComponent<TMP_InputField>().text;
        if (UserName.Length > 0 && _password.Length > 0)
        {
            if (IsValidUsername(UserName))
            {
                SC_LoginServerApi.Instance.Login(UserName, _password);
                MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Sent...";
            }
            else  MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Invalid Username, try again...";
        }
        else MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Missing Username/Password";
    }
    public void Btn_Login_Register()
    {
        Debug.Log("Btn_Login_Register");
        MainMenu_Logic.unityObjects["Screen_Register"].SetActive(true);
        MainMenu_Logic.unityObjects["Screen_Login"].SetActive(false);
        MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;
        MainMenu_Logic.unityObjects["InputField_Login_Username"].GetComponent<TMP_InputField>().text = string.Empty;
        MainMenu_Logic.unityObjects["InputField_Register_Password"].GetComponent<TMP_InputField>().text = string.Empty;
        MainMenu_Logic.unityObjects["InputField_Register_Password_Repeat"].GetComponent<TMP_InputField>().text = string.Empty;
    }
    public void Btn_Register_Register()
    {
        Debug.Log("Btn_Register_Register");
        MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;

        string _username =  MainMenu_Logic.unityObjects["InputField_Register_Username"].GetComponent<TMP_InputField>().text;
        string _password =  MainMenu_Logic.unityObjects["InputField_Register_Password"].GetComponent<TMP_InputField>().text;
        string _passwordRepeat =  MainMenu_Logic.unityObjects["InputField_Register_Password_Repeat"].GetComponent<TMP_InputField>().text;
        if (_password == _passwordRepeat && _password.Length > 0)
        {
            if(IsValidUsername(_username))
            {
                Dictionary<string, object> _data = new Dictionary<string, object>();
                _data.Add("Username", _username);
                _data.Add("Password", _password);
                SC_LoginServerApi.Instance.Register(_data);
                MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Sent...";
            }
            else MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Invalid Username, try again...";
        }
        else MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Password dont match..."; 
    }
    public void Btn_Register_Back()
    {
        Debug.Log("Btn_Register_Back");
        MainMenu_Logic.unityObjects["Screen_Register"].SetActive(false);
        MainMenu_Logic.unityObjects["Screen_Login"].SetActive(true);
        MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;
        MainMenu_Logic.unityObjects["InputField_Login_Username"].GetComponent<TMP_InputField>().text = string.Empty;
        MainMenu_Logic.unityObjects["InputField_Login_Password"].GetComponent<TMP_InputField>().text = string.Empty;
    }
    // public void Btn_Lobby_Logout()
    // {
    //     Debug.Log("Btn_Lobby_Logout");
    //     MainMenu_Logic.unityObjects["Screen_Lobby"].SetActive(false);
    //     MainMenu_Logic.unityObjects["Screen_Login"].SetActive(true);
    //     MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;
    //     MainMenu_Logic.unityObjects["InputField_Login_Username"].GetComponent<TMP_InputField>().text = string.Empty;
    //     MainMenu_Logic.unityObjects["InputField_Login_Password"].GetComponent<TMP_InputField>().text = string.Empty;
    // }
    // public void Btn_Lobby_AddXp()
    // {
    //     string _value = MainMenu_Logic.unityObjects["InputField_Xp"].GetComponent<TMP_InputField>().text;
    //     if (_value.Length > 0)
    //     {
    //         Dictionary<string, object> _data = new Dictionary<string, object>();
    //         _data.Add("Email", UserName);
    //         _data.Add("XpAmount", _value);
    //         SC_LoginServerApi.Instance.AddXp(_data);
    //     }
    //     else MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Xp amount value is empty";
    //
    // }
    // public void Btn_Lobby_AddCurrency()
    // {
    //     string _value = MainMenu_Logic.unityObjects["InputField_Currency"].GetComponent<TMP_InputField>().text;
    //     if (_value.Length > 0)
    //     {
    //         Dictionary<string, object> _data = new Dictionary<string, object>();
    //         _data.Add("Email", UserName);
    //         _data.Add("CurrencyAmount", _value); 
    //         SC_LoginServerApi.Instance.AddCurrency(_data);
    //     }
    //     else MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Currency amount value is empty";
    // }
    // public void Btn_Lobby_Play()
    // {
    //     Debug.Log("Btn_Lobby_Play");
    // //    unityObjects["Btn_Lobby_Play"].GetComponent<Button>().interactable = false;
    //     SC_LoginServerApi.Instance.SearchingOpponent(userId);
    // }
    
    #endregion

    #region Events

    private void OnGeneralResponse(Dictionary<string, object> _Data)
    {
        if (_Data.ContainsKey("Response"))
        {
            switch (_Data["Response"].ToString())
            {
                #region Register
                case "Register": RegisterResponse(_Data); break;
                #endregion
                #region Login
                case "Login": LoginResponse(_Data); break;
                #endregion
                #region Add Xp
                case "AddXp": AddXpResponse(_Data); break;
                #endregion
                #region Add Currency
                case "AddCurrency": AddCurrencyResponse(_Data); break;
                #endregion
                #region Searching Opponent 
                // case "SearchingOpponent": SearchingOpponentResponse(_Data); break;
                #endregion
            }
        }
    }

    // private void OnConnect(Dictionary<string, object> _Data)
    // {
    //     Debug.Log("OnConnect ");
    //     if (_Data.ContainsKey("IsOpen"))
    //     {
    //         bool _isOpen = bool.Parse(_Data["IsOpen"].ToString());
    //         if (_isOpen)
    //         {
    //             MainMenu_Logic.unityObjects["Screen_Lobby"].SetActive(false);
    //             MainMenu_Logic.unityObjects["Screen_SearchingOpponent"].SetActive(true);
    //         }
    //     }
    // }
    // private void OnStartGame(Dictionary<string, object> _PassedVariables)
    // {
    //     MainMenu_Logic.unityObjects["Screen_SearchingOpponent"].SetActive(false);
    //     MainMenu_Logic.unityObjects["Menu"].SetActive(false);
    //     MainMenu_Logic.unityObjects["Game"].SetActive(true);
    //
    //     int _matchId = int.Parse(_PassedVariables["MI"].ToString());
    //     int _maxTurnTime = int.Parse(_PassedVariables["MTT"].ToString());
    //     int _moveCounter = int.Parse(_PassedVariables["MC"].ToString());
    //     System.DateTime _startTime = System.DateTime.Parse(_PassedVariables["TT"].ToString());
    //     string _curPlayerId = _PassedVariables["CP"].ToString();
    //     List<object> players = (List<object>)_PassedVariables["Players"];
    //     List<string> stringPlayers = players.Select(s => (string)s).ToList();
    //     // SC_GameLogic.Instance.StartGame(_matchId, _maxTurnTime, _startTime, _curPlayerId, _moveCounter, userId, stringPlayers);
    // }
    private void OnSendMove(Dictionary<string, object> _PassedVariables)
    {
        Debug.Log(_PassedVariables.Count);
    }
    
    #endregion

    #region Logic 

    bool IsValidUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return false;

        var trimmedUsername = username.Trim();

        // Check length (3-20 characters)
        if (trimmedUsername.Length < 2 || trimmedUsername.Length > 15)
            return false;

        // Ensure it starts and ends with a valid character (letter or digit)
        if (!char.IsLetterOrDigit(trimmedUsername[0]) || !char.IsLetterOrDigit(trimmedUsername[^1]))
            return false;

        // Check for invalid patterns using regex
        if (System.Text.RegularExpressions.Regex.IsMatch(trimmedUsername, @"[^a-zA-Z0-9._]")) // Invalid characters
            return false;
        if (trimmedUsername.Contains("..") || trimmedUsername.Contains("__")) // No consecutive dots or underscores
            return false;

        return true;
    }


    private void RegisterResponse(Dictionary<string, object> _Data)
    {
        Debug.Log("RegisterResponse " + _Data.Count);
        MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;
        if (_Data.ContainsKey("ErrorCode"))
        {
            MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = _Data["ErrorCode"].ToString();
        }
        else if (_Data.ContainsKey("IsCreated"))
        {
            bool _isCreated = bool.Parse(_Data["IsCreated"].ToString());
            if(_isCreated)
            {
                MainMenu_Logic.unityObjects["Screen_Register"].SetActive(false);
                MainMenu_Logic.unityObjects["Screen_Login"].SetActive(true);
                MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "User was created!";
            }
            else MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Failed to create a user";
        }
    }
    private void LoginResponse(Dictionary<string, object> _Data)
    {
        Debug.Log("RegisterResponse " + _Data.Count);
        
        MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;
        if (_Data.ContainsKey("ErrorCode"))
        {
            MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = _Data["ErrorCode"].ToString();
        }
        else if (_Data.ContainsKey("IsLoggedIn"))
        {
            bool _isLoggedIn = bool.Parse(_Data["IsLoggedIn"].ToString());
            if (_isLoggedIn)
            {
                userId = _Data["UserId"].ToString();
                UserName = MainMenu_Logic.unityObjects["InputField_Login_Username"].GetComponent<TMP_InputField>().text;
                Debug.Log("UserName" + UserName +"userId " + userId);
                if (MainMenu_Logic.unityObjects.ContainsKey("UserId"))
                    MainMenu_Logic.unityObjects["UserId"].GetComponent<TextMeshProUGUI>().text = userId;
                MainMenu_Logic.unityObjects["Screen_Login"].SetActive(false);
                MainMenu_Logic.unityObjects["Screen_MainMenu"].SetActive(true);
                MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Logged in";
            }
            else MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Failed to log in";
        }
    }

    private void AddCurrencyResponse(Dictionary<string, object> _Data)
    {
        MainMenu_Logic.unityObjects["InputField_Box"].GetComponent<TMP_InputField>().text = string.Empty;
        foreach (string s in _Data.Keys)
            MainMenu_Logic.unityObjects["InputField_Box"].GetComponent<TMP_InputField>().text += s + " : " + _Data[s] + System.Environment.NewLine;
    }

    private void AddXpResponse(Dictionary<string, object> _Data)
    {
        MainMenu_Logic.unityObjects["InputField_Box"].GetComponent<TMP_InputField>().text = string.Empty;
        foreach (string s in _Data.Keys)
            MainMenu_Logic.unityObjects["InputField_Box"].GetComponent<TMP_InputField>().text += s + " : " + _Data[s] + System.Environment.NewLine;
    }
    
    #endregion
}
