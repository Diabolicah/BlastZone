using System.Collections.Generic;
using Fusion;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectionUi : MonoBehaviour
{
    public bool IsShowing { get; private set; } = false;

    [SerializeField] private GameObject cardButtonPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Transform cardSelectionContainer;
    public CardManager cardManager;

    private void Awake()
    {
        cardSelectionContainer.gameObject.SetActive(false);
    }

    public void ShowLevelUpOptions(List<CardConfig> cardOptions)
    {
        cardSelectionContainer.gameObject.SetActive(true);
        IsShowing = true;

        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }
        float counter = -1;
        foreach (var card in cardOptions)
        {
            GameObject cardButtonObj = Instantiate(cardButtonPrefab, cardContainer);
            CardButton cardButton = cardButtonObj.GetComponent<CardButton>();
            cardButton.Setup(card, OnCardSelected);
            cardButtonObj.transform.localPosition = new Vector3(300*counter, 0, 0);
            counter++;
        }
    }

    private void OnCardSelected(CardConfig selectedCard)
    {
        Hide();
        cardManager?.OnCardSelected(selectedCard);
    }

    public void Hide()
    {
        cardSelectionContainer.gameObject.SetActive(false);
        IsShowing = false;
    }

}
