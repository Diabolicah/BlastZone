using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class CardManager : NetworkBehaviour
{
    [SerializeField] private List<CardConfig> availableCards;
    [SerializeField] private CardSelectionUi CardSelectionUI;
    [SerializeField] private CardFunctions CardFunctions;

    private int CardCountPerSelection = 3;
    public List<int> pendingCardSelections = new List<int>();

    private void Start()
    {
        if (CardSelectionUI == null)
            throw new System.Exception("CardSelectionUI is not set");
        if (CardFunctions == null)
            throw new System.Exception("CardFunctions is not set");
    }

    private List<CardConfig> GetAvailableCards(int rank)
    {
        List<CardConfig> cards = new List<CardConfig>();
        foreach (var card in availableCards)
        {
            if (card.GetMinimumRank() <= rank)
            {
                cards.Add(card);
            }
        }
        return cards;
    }

    private List<CardConfig> GetRandomCardOptions(int rank)
    {
        List<CardConfig> cardList = GetAvailableCards(rank);
        if (cardList.Count < 0)
            throw new System.Exception("No cards available for rank " + rank);

        List<CardConfig> selectedCard = new List<CardConfig>();
        while (selectedCard.Count < CardCountPerSelection)
        {
            int index = UnityEngine.Random.Range(0, availableCards.Count);
            selectedCard.Add(availableCards[index]);
        }
        return selectedCard;
    }

    private void ShowLevelUpOptionsForNextLevel()
    {
        if (pendingCardSelections.Count <= 0)
            return;
        if (CardSelectionUI == null)
            throw new System.Exception("CardSelectionUI is not set");
        if (CardSelectionUI.IsShowing)
            return;
        List<CardConfig> cardOptions = GetRandomCardOptions(pendingCardSelections[0]);
        CardSelectionUI.ShowLevelUpOptions(cardOptions);
    }

    public void OnCardSelected(CardConfig selectedCard)
    {
        StatsManager statsManager = Runner.GetPlayerObject(Runner.LocalPlayer).GetComponent<StatsManager>();
        TankWeapons tankWeapons = Runner.GetPlayerObject(Runner.LocalPlayer).GetComponent<TankWeapons>();

        CardFunctions.ActivateCard(selectedCard, statsManager, tankWeapons);
        pendingCardSelections.RemoveAt(0);
        if (pendingCardSelections.Count > 0)
            ShowLevelUpOptionsForNextLevel();
    }

    public void AddCardSelection(int rank)
    {
        pendingCardSelections.Add(rank);
        ShowLevelUpOptionsForNextLevel();
    }
}
