using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI type;
    [SerializeField] private TextMeshProUGUI minimumRank;

    private CardConfig cardConfig;
    private System.Action<CardConfig> onCardSelected;

    public void Setup(CardConfig config, System.Action<CardConfig> callback)
    {
        cardConfig = config;
        onCardSelected = callback;
        if (titleText != null) titleText.text = config.GetTitle();
        if (cardImage != null) cardImage.sprite = config.GetImage();
        if (descriptionText != null) descriptionText.text = config.GetDescription();
        if (type != null) type.text = config.GetCardType().ToString();
        if (minimumRank != null) minimumRank.text = config.GetMinimumRank().ToString();
    }

    public void OnButtonClicked()
    {
        onCardSelected?.Invoke(cardConfig);
    }
}
