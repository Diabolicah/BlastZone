using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    [SerializeField] private GameObject titleText;
    [SerializeField] private GameObject cardImage;
    [SerializeField] private GameObject descriptionText;
    [SerializeField] private GameObject type;
    private CardConfig cardConfig;
    private System.Action<CardConfig> onCardSelected;

    public void Setup(CardConfig config, System.Action<CardConfig> callback)
    {
        cardConfig = config;
        onCardSelected = callback;
        if (titleText != null) titleText.GetComponent<TextMeshPro>().text = config.GetTitle();
        if (cardImage != null) cardImage.GetComponent<Image>().sprite = config.GetImage();
        if (descriptionText != null) descriptionText.GetComponent<TextMeshPro>().text = config.GetDescription();
        if (type != null) type.GetComponent<TextMeshPro>().text = config.GetCardType().ToString();
    }

    public void OnButtonClicked()
    {
        onCardSelected?.Invoke(cardConfig);
    }
}
