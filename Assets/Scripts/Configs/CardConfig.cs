using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Card Config")]
public class CardConfig : ScriptableObject
{
    public enum CardType
    {
        Stat,
        Ability,
        Element,
        Temporary
    }

    [SerializeField] private string Title;
    [SerializeField] private string Description;
    [SerializeField] private Sprite Image;
    [SerializeField] private int MinimumRank;
    [SerializeField] private CardType type;

    public string GetTitle()
    {
        return Title;
    }

    public string GetDescription()
    {
        return Description;
    }

    public Sprite GetImage()
    {
        return Image;
    }

    public int GetMinimumRank()
    {
        return MinimumRank;
    }

    public CardType GetCardType()
    {
        return type;
    }
}