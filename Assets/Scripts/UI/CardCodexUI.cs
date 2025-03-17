using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardCodexUI : MonoBehaviour
{
    public bool IsShowing { get; private set; } = false;

    [SerializeField] private GameObject cardButtonPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private float horizontalMargin = 20f;
    [SerializeField] private float verticalMargin = 20f;
    [SerializeField] private TextMeshProUGUI CurrentRank;
    [SerializeField] public CardManager cardManager;

    public void AggregateCodex(int rank, List<CardConfig> cardOptions)
    {
        CurrentRank.text = "CurrentRank: " + rank;
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        cardOptions.Sort((a, b) => a.GetMinimumRank().CompareTo(b.GetMinimumRank()));

        // Get the card dimensions from the prefab.
        float cardWidth = cardButtonPrefab.GetComponent<RectTransform>().rect.width;
        float cardHeight = cardButtonPrefab.GetComponent<RectTransform>().rect.height;

        // Get the viewport's RectTransform (assuming cardContainer is a child of the viewport).
        RectTransform viewportRect = cardContainer.parent.GetComponent<RectTransform>();
        float viewportWidth = viewportRect.rect.width;

        // Effective dimensions including margins.
        float effectiveCardWidth = cardWidth + horizontalMargin;
        float effectiveCardHeight = cardHeight + verticalMargin;

        // Calculate how many columns fit in the viewport.
        int columns = Mathf.Max(1, Mathf.FloorToInt((viewportWidth + horizontalMargin) / effectiveCardWidth));
        int cardCount = cardOptions.Count;
        int rows = Mathf.CeilToInt((float)cardCount / columns);

        // Calculate the overall grid dimensions.
        float gridWidth = columns * cardWidth + (columns - 1) * horizontalMargin;
        float gridHeight = rows * cardHeight + (rows - 1) * verticalMargin;

        // Update the container's size to fit the grid.
        RectTransform containerRect = cardContainer.GetComponent<RectTransform>();
        if (containerRect != null)
        {
            containerRect.sizeDelta = new Vector2(viewportWidth, gridHeight);
        }

        // Calculate starting positions so that the grid is centered.
        float startX = -gridWidth / 2f + cardWidth / 2f;
        float startY = gridHeight / 2f - cardHeight / 2f;

        // Instantiate and position each card.
        for (int i = 0; i < cardCount; i++)
        {
            int row = i / columns;
            int column = i % columns;

            float posX = startX + column * (cardWidth + horizontalMargin);
            float posY = startY - row * (cardHeight + verticalMargin);

            GameObject cardButtonObj = Instantiate(cardButtonPrefab, cardContainer);
            CardButton cardButton = cardButtonObj.GetComponent<CardButton>();
            cardButton.Setup(cardOptions[i], Show);
            cardButtonObj.transform.localPosition = new Vector3(posX, posY, 0);

            if (cardOptions[i].GetMinimumRank() > rank)
            {
                GameObject dimmer = cardButtonObj.transform.Find("dimmer").gameObject;
                dimmer.SetActive(true);
            }
        }
    }




    public void Show(CardConfig cardConfig)
    {

    }

}
