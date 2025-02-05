using System.Collections.Generic;
using Fusion;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : NetworkBehaviour
{
    public bool IsShowing { get; private set; } = false;

    [SerializeField] private GameObject cardButtonPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Transform LevelUpUIContainer;
    public LevelingManager levelingManager;

    private void Awake()
    {
        LevelUpUIContainer.gameObject.SetActive(false);
    }

    public void ShowLevelUpOptions(List<CardConfig> cardOptions)
    {
        LevelUpUIContainer.gameObject.SetActive(true);
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

    // Callback when a card is selected.
    private void OnCardSelected(CardConfig selectedCard)
    {
        levelingManager.OnCardSelected(selectedCard);
        if (levelingManager != null && levelingManager.pendingLevelUps <= 0)
        {
            Hide();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        IsShowing = false;
    }

}
