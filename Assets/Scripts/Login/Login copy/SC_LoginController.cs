using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_LoginController : MonoBehaviour
{
    public void Btn_Login_Login()
    {
        SC_LoginLogic.Instance.Btn_Login_Login();
    }
    public void Btn_Login_Register()
    {
        SC_LoginLogic.Instance.Btn_Login_Register();
    }
    public void Btn_Register_Register()
    {
        SC_LoginLogic.Instance.Btn_Register_Register();
    }
    public void Btn_Register_Back()
    {
        SC_LoginLogic.Instance.Btn_Register_Back();
    }
    public void Btn_Lobby_Logout()
    {
        SC_LoginLogic.Instance.Btn_Lobby_Logout();
    }

    public void Btn_Lobby_AddXp()
    {
        SC_LoginLogic.Instance.Btn_Lobby_AddXp();
    }
    public void Btn_Lobby_AddCurrency()
    {
        SC_LoginLogic.Instance.Btn_Lobby_AddCurrency();
    }
    public void Btn_Lobby_Play()
    {
        SC_LoginLogic.Instance.Btn_Lobby_Play();
    }
    public void Btn_SearchingOpponent_Cancel()
    {
        SC_LoginLogic.Instance.Btn_SearchingOpponent_Cancel();
    }

    public void Btn_SearchingOpponent_SendMessage()
    {
        SC_LoginLogic.Instance.Btn_SearchingOpponent_SendMessage();
    }
    
}
