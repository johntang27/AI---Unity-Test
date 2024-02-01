using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerCard
{
    public CardSuit cardSuit;
    public CardValue cardValue;
    public Sprite cardSprite;
    public int blackjackValue;

    public PlayerCard(CardScriptableObject scriptableObject)
    {
        cardSuit = scriptableObject.GetSuit;
        cardValue = scriptableObject.GetValue;
        cardSprite = scriptableObject.GetSprite;
        blackjackValue = scriptableObject.GetBlackjackCardValue;
    }
}

/// <summary>
/// Contains the cards currently in player's hand, inherited by DealerHandScriptableObject
/// Also reference the DeckScriptableObject that will be drawing cards from
/// </summary>
[CreateAssetMenu(fileName = "PlayerHandScriptableObject", menuName = "ScriptableObjects/PlayerHandScriptableObject")]
public class PlayerHandScriptableObject : ScriptableObject
{
    [SerializeField] protected List<PlayerCard> currentHand = new List<PlayerCard>();
    [SerializeField] protected DeckScriptableObject deck = null;

    public List<PlayerCard> GetCurrentHand => currentHand;

    public virtual void InitializeHand(int handSize)
    {
        if (deck != null)
        {
            deck.ShuffleDeck();
            currentHand = new List<PlayerCard>();
            for (int i = 0; i < handSize; i++)
            {
                PlayerCard newCard = new PlayerCard(deck.GetNextCard());
                //Debug.LogError("Adding card: " + newCard.cardValue + " of " + newCard.cardSuit);
                currentHand.Add(newCard);
            }
        }
    }
    
    public void ReplaceCardFromDeck(int cardIndex)
    {
        if(cardIndex > currentHand.Count - 1)
        {
            Debug.LogErrorFormat("Card Index is out of range, cannot draw card from deck to replace card index: {0} in hand", cardIndex);
            return;
        }

        PlayerCard playerCard = new PlayerCard(deck.GetNextCard());
        currentHand[cardIndex] = playerCard;
    }
}
