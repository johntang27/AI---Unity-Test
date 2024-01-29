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

[CreateAssetMenu(fileName = "PlayerHandScriptableObject", menuName = "ScriptableObjects/PlayerHandScriptableObject")]
public class PlayerHandScriptableObject : ScriptableObject
{
    [SerializeField] protected List<PlayerCard> currentHand = new List<PlayerCard>();
    [SerializeField] protected DeckScriptableObject deck = null;

    public List<PlayerCard> GetCurrentHand => currentHand;

    protected int nextCardIndex = -1;

    public virtual void InitializeHand(int handSize)
    {
        if (deck != null)
        {
            deck.ShuffleDeck();
            currentHand = new List<PlayerCard>();
            for (int i = 0; i < handSize; i++)
            {
                PlayerCard newCard = new PlayerCard(deck.GetDeck[i]);
                //Debug.LogError("Adding card: " + newCard.cardValue + " of " + newCard.cardSuit);
                currentHand.Add(newCard);
            }
            nextCardIndex = handSize;
        }
    }
    
    public void ReplaceCardFromDeck(int cardIndex)
    {
        if(cardIndex > currentHand.Count - 1)
        {
            Debug.LogErrorFormat("Card Index is out of range, cannot draw card from deck to replace card index: {0} in hand", cardIndex);
            return;
        }

        PlayerCard playerCard = new PlayerCard(deck.GetDeck[nextCardIndex]);
        currentHand[cardIndex] = playerCard;

        nextCardIndex++;
    }
}
