using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UI.MainMenu_Logic;

namespace UI
{
    public class MainMenuHandler : MonoBehaviour
    {
        #region Variables
        public MainMenu_Logic curMenuLogic;
        public CardCodexUI cardCodexUI;
        #endregion

        #region Logic
        public void Btn_ChangeScreen(string _ScreenName)
        {
            if (curMenuLogic != null)
            {
                if(_ScreenName == "back")
                {
                    Flag = -1;
                    curMenuLogic.ChangeScreen(curMenuLogic.Screens_Stack.Pop());
                }
                else
                {
                    if (_ScreenName == "Customization")
                    {
                        unityObjects["TankColorModel"].SetActive(true);
                    }
                    if (_ScreenName == "Codex")
                    {
                        int rank = 0;
                        bool isRankInt= int.TryParse(PlayerPrefs.GetString("PlayerRank"), out rank);
                        if (!isRankInt)
                        {
                            rank = 0;
                        }
                        cardCodexUI.AggregateCodex(rank, cardCodexUI.cardManager.availableCards);
                    }
                    Flag = 1;
                    Screens _toScreen = (Screens)Enum.Parse(typeof(Screens), _ScreenName);
                    curMenuLogic.ChangeScreen(_toScreen);
                }
            }
        }
        #endregion
        }
}