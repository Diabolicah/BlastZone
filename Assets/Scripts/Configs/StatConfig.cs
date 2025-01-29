using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Stat Config")]
public class StatConfig : ScriptableObject
{
    [SerializeField] private string StatName {get; set;} = "";
    [SerializeField] private float DefaultStatValue { get; set; } = 0;
    [SerializeField] private float MaxStatValue { get; set; } = 0;
    [SerializeField][Networked] private string CurrentStatValueName { get; set; } = "";


    public string GetStatName()
    {
        return StatName;
    }

    public float GetDefaultStatValue()
    {
        return DefaultStatValue;
    }

    public float GetMaxStatValue()
    {
        return MaxStatValue;
    }

    public string GetCurrentStatValueName()
    {
        return CurrentStatValueName;
    }

    public void SetCurrentStatValueName(string value)
    {
        CurrentStatValueName = value;
    }
}
