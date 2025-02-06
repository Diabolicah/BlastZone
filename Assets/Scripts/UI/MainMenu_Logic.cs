using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
// using AssemblyCSharp;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu_Logic : MonoBehaviour
    {
        public enum Screens
    {
        MainMenu, Options, Multiplayer, Codex, Customization, ChooseCard, Game , GameOver
    };

    #region Variables

    public static int Flag;
    public static Dictionary<string, GameObject> unityObjects;
    public  Stack <Screens> Screens_Stack = new Stack<Screens>();
    private Screens curScreen;
    private Screens prevScreen;
    #endregion

    #region Singleton

    private static MainMenu_Logic instance;
    public static MainMenu_Logic Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("MainMenu_Logic").GetComponent<MainMenu_Logic>();

            return instance;
        }
    }

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        InitAwake();
    }

    private void Start()
    {
        InitStart();
    }

    #endregion

    #region Logic

    private void InitAwake()
    {
        curScreen = Screens.MainMenu;
        prevScreen = Screens.MainMenu;
        unityObjects = new Dictionary<string, GameObject>();
        GameObject[] _unityObj = GameObject.FindGameObjectsWithTag("UnityObject");
        foreach(GameObject g in _unityObj)
        {
            if (unityObjects.ContainsKey(g.name) == false)
                unityObjects.Add(g.name, g);
            else Debug.LogError("This key " + g.name + " Is Already inside the Dictionary!!!");
        }
        
    }

    private void InitStart()
    {
        if (unityObjects.ContainsKey("Screen_MainMenu"))
            unityObjects["Screen_MainMenu"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_Multiplayer") && !PlayerPrefs.HasKey("Restart"))
            unityObjects["Screen_Multiplayer"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_Codex"))
            unityObjects["Screen_Codex"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_Customization"))
            unityObjects["Screen_Customization"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_Options"))
            unityObjects["Screen_Options"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_ChooseCard"))
            unityObjects["Screen_ChooseCard"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_Game"))
            unityObjects["Screen_Game"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_GameOver"))
            unityObjects["Screen_GameOver"].SetActive(false);
        
    }

    public void ChangeScreen(Screens _ToScreen)
    {
        prevScreen = curScreen;
        curScreen = _ToScreen;
        if (Flag == 1)
        {
            Screens_Stack.Push(prevScreen);
        }
        if (unityObjects.ContainsKey("Screen_" + prevScreen))
            unityObjects["Screen_" + prevScreen].SetActive(false);

        if (unityObjects.ContainsKey("Screen_" + curScreen))
            unityObjects["Screen_" + curScreen].SetActive(true);
    }

    #endregion

    }
}