using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SC_LoginLogic : MonoBehaviour
{
    public Dictionary<string, GameObject> unityObjects;
    private string email = string.Empty;
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
        unityObjects = new Dictionary<string, GameObject>();
        GameObject[] _objects = GameObject.FindGameObjectsWithTag("UnityObject");
        foreach (GameObject g in _objects)
            unityObjects.Add(g.name, g);

        unityObjects["Screen_Register"].SetActive(false);
        unityObjects["Screen_Lobby"].SetActive(false);
        //unityObjects["Screen_SearchingOpponent"].SetActive(false);
        //unityObjects["Game"].SetActive(false);
    }
    #endregion


    #region Controller
    public void Btn_Login_Login()
    {
        Debug.Log("Btn_Login_Login");
        unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;

        email = unityObjects["InputField_Login_Email"].GetComponent<TMP_InputField>().text;
        string _password = unityObjects["InputField_Login_Password"].GetComponent<TMP_InputField>().text;
        if (email.Length > 0 && _password.Length > 0)
        {
            if (IsValidEmail(email))
            {
                SC_LoginServerApi.Instance.Login(email, _password);
                unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Sent...";
            }
            else unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Broken email...";
        }
        else unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Missing Email/Password";
    }
    public void Btn_Login_Register()
    {
        Debug.Log("Btn_Login_Register");
        unityObjects["Screen_Register"].SetActive(true);
        unityObjects["Screen_Login"].SetActive(false);
        unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;
        unityObjects["InputField_Register_Email"].GetComponent<TMP_InputField>().text = string.Empty;
        unityObjects["InputField_Register_Password"].GetComponent<TMP_InputField>().text = string.Empty;
        unityObjects["InputField_Register_Password_Repeat"].GetComponent<TMP_InputField>().text = string.Empty;
    }
    public void Btn_Register_Register()
    {
        Debug.Log("Btn_Register_Register");
        unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;

        string _email = unityObjects["InputField_Register_Email"].GetComponent<TMP_InputField>().text;
        string _password = unityObjects["InputField_Register_Password"].GetComponent<TMP_InputField>().text;
        string _passwordRepeat = unityObjects["InputField_Register_Password_Repeat"].GetComponent<TMP_InputField>().text;
        if (_password == _passwordRepeat && _password.Length > 0)
        {
            if(IsValidEmail(_email))
            {
                Dictionary<string, object> _data = new Dictionary<string, object>();
                _data.Add("Email", _email);
                _data.Add("Password", _password);
                SC_LoginServerApi.Instance.Register(_data);
                unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Sent...";
            }
            else unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Broken email...";
        }
        else unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Password dont match..."; 
    }
    public void Btn_Register_Back()
    {
        Debug.Log("Btn_Register_Back");
        unityObjects["Screen_Register"].SetActive(false);
        unityObjects["Screen_Login"].SetActive(true);
        unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;
        unityObjects["InputField_Login_Email"].GetComponent<TMP_InputField>().text = string.Empty;
        unityObjects["InputField_Login_Password"].GetComponent<TMP_InputField>().text = string.Empty;
    }
    public void Btn_Lobby_Logout()
    {
        Debug.Log("Btn_Lobby_Logout");
        unityObjects["Screen_Lobby"].SetActive(false);
        unityObjects["Screen_Login"].SetActive(true);
        unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;
        unityObjects["InputField_Login_Email"].GetComponent<TMP_InputField>().text = string.Empty;
        unityObjects["InputField_Login_Password"].GetComponent<TMP_InputField>().text = string.Empty;
    }
    public void Btn_Lobby_AddXp()
    {
        string _value = unityObjects["InputField_Xp"].GetComponent<TMP_InputField>().text;
        if (_value.Length > 0)
        {
            Dictionary<string, object> _data = new Dictionary<string, object>();
            _data.Add("Email", email);
            _data.Add("XpAmount", _value);
            SC_LoginServerApi.Instance.AddXp(_data);
        }
        else unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Xp amount value is empty";

    }
    public void Btn_Lobby_AddCurrency()
    {
        string _value = unityObjects["InputField_Currency"].GetComponent<TMP_InputField>().text;
        if (_value.Length > 0)
        {
            Dictionary<string, object> _data = new Dictionary<string, object>();
            _data.Add("Email", email);
            _data.Add("CurrencyAmount", _value); 
            SC_LoginServerApi.Instance.AddCurrency(_data);
        }
        else unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Currency amount value is empty";
    }
    public void Btn_Lobby_Play()
    {
        Debug.Log("Btn_Lobby_Play");
    //    unityObjects["Btn_Lobby_Play"].GetComponent<Button>().interactable = false;
        SC_LoginServerApi.Instance.SearchingOpponent(userId);
    }

    public void Btn_SearchingOpponent_Cancel()
    {
        Debug.Log("Btn_SearchingOpponent_Cancel");
        unityObjects["Btn_SearchingOpponent_Cancel"].GetComponent<Button>().interactable = false;
        // SC_WebSocket.Instance.CancelMatching();
    }

    public void Btn_SearchingOpponent_SendMessage()
    {
        Debug.Log("Btn_SearchingOpponent_Cancel");
        string _message = unityObjects["InputField_SearchingOpponent_Message"].GetComponent<TMP_InputField>().text;
        Dictionary<string, object> _data = new Dictionary<string, object>();
        _data.Add("Msg", _message);
        // SC_WebSocket.Instance.SendServerMessage(_data);
    }
    
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

    private void OnConnect(Dictionary<string, object> _Data)
    {
        Debug.Log("OnConnect ");
        if (_Data.ContainsKey("IsOpen"))
        {
            bool _isOpen = bool.Parse(_Data["IsOpen"].ToString());
            if (_isOpen)
            {
                unityObjects["Screen_Lobby"].SetActive(false);
                unityObjects["Screen_SearchingOpponent"].SetActive(true);
            }
        }
    }
    private void OnDisconnect(Dictionary<string, object> _Data)
    {
        Debug.Log("OnDisconnect ");
        unityObjects["Screen_Lobby"].SetActive(true);
        unityObjects["Screen_SearchingOpponent"].SetActive(false);
        unityObjects["Btn_Lobby_Play"].GetComponent<Button>().interactable = true;
    }
    
    private void OnCancelMatching(Dictionary<string, object> _Data)
    {
        Debug.Log("OnCancelMatching ");
        // SC_WebSocket.Instance.CloseConnection();
        unityObjects["Btn_SearchingOpponent_Cancel"].GetComponent<Button>().interactable = true;
        unityObjects["Btn_Lobby_Play"].GetComponent<Button>().interactable = true;
        unityObjects["Screen_Lobby"].SetActive(true);
        unityObjects["Screen_SearchingOpponent"].SetActive(false);
    }
    private void OnSendMessage(Dictionary<string, object> _Data)
    {
        Debug.Log("OnSendMessage");
    }
    private void OnBroadcastMessage(Dictionary<string, object> _Data)
    {
        Debug.Log("OnBroadcastMessage");
        foreach (string s in _Data.Keys)
            unityObjects["InputField_SearchingOpponent_Display"].GetComponent<TMP_InputField>().text += s + " : " + _Data[s] + System.Environment.NewLine;
    }
    private void OnReadyToPlay(Dictionary<string, object> _PassedVariables)
    {
        if (_PassedVariables.ContainsKey("TempMatchId"))
        {
            tempMatchId = _PassedVariables["TempMatchId"].ToString();
            // SC_WebSocket.Instance.ReadyToPlay(tempMatchId);
        }
    }
    private void OnStartGame(Dictionary<string, object> _PassedVariables)
    {
        unityObjects["Screen_SearchingOpponent"].SetActive(false);
        unityObjects["Menu"].SetActive(false);
        unityObjects["Game"].SetActive(true);

        int _matchId = int.Parse(_PassedVariables["MI"].ToString());
        int _maxTurnTime = int.Parse(_PassedVariables["MTT"].ToString());
        int _moveCounter = int.Parse(_PassedVariables["MC"].ToString());
        System.DateTime _startTime = System.DateTime.Parse(_PassedVariables["TT"].ToString());
        string _curPlayerId = _PassedVariables["CP"].ToString();
        List<object> players = (List<object>)_PassedVariables["Players"];
        List<string> stringPlayers = players.Select(s => (string)s).ToList();
        // SC_GameLogic.Instance.StartGame(_matchId, _maxTurnTime, _startTime, _curPlayerId, _moveCounter, userId, stringPlayers);
    }
    private void OnSendMove(Dictionary<string, object> _PassedVariables)
    {
        Debug.Log(_PassedVariables.Count);
    }
    
    #endregion

    #region Logic 

    bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false; // suggested by @TK-421
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    private void RegisterResponse(Dictionary<string, object> _Data)
    {
        Debug.Log("RegisterResponse " + _Data.Count);
        unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;
        if (_Data.ContainsKey("ErrorCode"))
        {
            unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = _Data["ErrorCode"].ToString();
        }
        else if (_Data.ContainsKey("IsCreated"))
        {
            bool _isCreated = bool.Parse(_Data["IsCreated"].ToString());
            if(_isCreated)
            {
                unityObjects["Screen_Register"].SetActive(false);
                unityObjects["Screen_Login"].SetActive(true);
                unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "User was created!";
            }
            else unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Failed to create a user";
        }
    }
    private void LoginResponse(Dictionary<string, object> _Data)
    {
        Debug.Log("RegisterResponse " + _Data.Count);
        unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = string.Empty;
        if (_Data.ContainsKey("ErrorCode"))
        {
            unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = _Data["ErrorCode"].ToString();
        }
        else if (_Data.ContainsKey("IsLoggedIn"))
        {
            bool _isLoggedIn = bool.Parse(_Data["IsLoggedIn"].ToString());
            if (_isLoggedIn)
            {
                userId = _Data["UserId"].ToString();
                Debug.Log("userId " + userId);
                if(unityObjects.ContainsKey("UserId"))
                    unityObjects["UserId"].GetComponent<TextMeshProUGUI>().text = userId;
                email = unityObjects["InputField_Login_Email"].GetComponent<TMP_InputField>().text;
                unityObjects["Screen_Login"].SetActive(false);
                unityObjects["Screen_Lobby"].SetActive(true);
                unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Logged in";
            }
            else unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Failed to log in";
        }
    }

    private void AddCurrencyResponse(Dictionary<string, object> _Data)
    {
        unityObjects["InputField_Box"].GetComponent<TMP_InputField>().text = string.Empty;
        foreach (string s in _Data.Keys)
            unityObjects["InputField_Box"].GetComponent<TMP_InputField>().text += s + " : " + _Data[s] + System.Environment.NewLine;
    }

    private void AddXpResponse(Dictionary<string, object> _Data)
    {
        unityObjects["InputField_Box"].GetComponent<TMP_InputField>().text = string.Empty;
        foreach (string s in _Data.Keys)
            unityObjects["InputField_Box"].GetComponent<TMP_InputField>().text += s + " : " + _Data[s] + System.Environment.NewLine;
    }
    
    #endregion
}
