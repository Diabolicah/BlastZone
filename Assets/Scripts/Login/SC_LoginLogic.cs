using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class SC_LoginLogic : NetworkBehaviour
{
    // public Dictionary<string, GameObject> unityObjects;
    private string UserName = string.Empty;
    private string userId = string.Empty;
    private string tempMatchId = "";
    public static string PLayerName = "";
    public static string PLayerRank = "";
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
    }

    private void OnDisable()
    {
        SC_LoginServerApi.OnGeneralResponse -= OnGeneralResponse;
    }

    private void Start()
    {
        MainMenu_Logic.unityObjects["Screen_Register"].SetActive(false);
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
                _data.Add("Email", _username);
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
    
    public void Btn_Lobby_AddXp(int Level)
    {
        string _value = Level.ToString();
        if (_value.Length > 0)
        {
            Dictionary<string, object> _data = new Dictionary<string, object>();
            _data.Add("Email", UserName);
            _data.Add("XpAmount", _value);
            SC_LoginServerApi.Instance.AddXp(_data);
        }
        else MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Xp amount value is empty";
    
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
            }
        }
    }
    
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
        if (trimmedUsername.Length < 1 || trimmedUsername.Length > 15)
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
                MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Logged in!";
                PLayerName = UserName;
            }
            else MainMenu_Logic.unityObjects["Txt_Error"].GetComponent<TextMeshProUGUI>().text = "Failed to log in";
        }
    }

    private void AddXpResponse(Dictionary<string, object> _Data)
    {
        MainMenu_Logic.unityObjects["InputField_Box"].GetComponent<TMP_InputField>().text = string.Empty;
        foreach (string s in _Data.Keys)
            MainMenu_Logic.unityObjects["InputField_Box"].GetComponent<TMP_InputField>().text += s + " : " + _Data[s] + System.Environment.NewLine;
    }
    
    #endregion
}
