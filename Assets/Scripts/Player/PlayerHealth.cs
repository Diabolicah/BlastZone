using System.Collections.Generic;
using UI;
using UnityEngine;
using static Unity.Collections.Unicode;

public class PlayerHealth : Health
{
    protected override void OnDeath()
    {
        base.OnDeath();
        int Level = Object.GetComponent<LevelingManager>().Level;
        Dictionary<string, object> _data = new Dictionary<string, object>();
        _data.Add("Email", GetComponent<Player>().Username);
        _data.Add("XpAmount", Level);
        SC_LoginServerApi.Instance.AddXp(_data);
        Runner.Shutdown();
        MainMenu_Logic.unityObjects["Screen_GameOver"].SetActive(true);
    }
}
