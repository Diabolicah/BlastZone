using System.Runtime.Serialization;
using Fusion;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Image LevelBar;

    [HideInInspector] public LevelingManager levelingManager;

    public void Activate(LevelingManager levelingManager)
    {
        this.levelingManager = levelingManager;
        UpdateStats(levelingManager.Exp, levelingManager.expToLevelUp, levelingManager.Level);
        levelingManager.OnStatsChanged += (exp, maxExp, level) => UpdateStats(exp, maxExp, level);
    }

    private void UpdateStats(float exp, float maxExp, int level)
    {
        if (levelingManager == null)
        {
            return;
        }
        LevelText.text = $"Level: {level}";
        LevelBar.fillAmount = exp / maxExp;
    }
}