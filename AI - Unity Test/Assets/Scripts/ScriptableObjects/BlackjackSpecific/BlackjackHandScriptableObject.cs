using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackjackHandScriptableObject", menuName = "ScriptableObjects/BlackjackHandScriptableObject")]
public class BlackjackHandScriptableObject : PlayerHandScriptableObject
{
    //when we initialize a new blackjack hand, we only want to shuffle the deck and initialize the hand(list)
    public override void InitializeHand(int handSize)
    {
        if (deck != null) deck.ShuffleDeck();
        currentHand = new List<PlayerCard>();
    }

    //add the next card from the deck to the hand
    public void DrawNextCardFromDeck()
    {
        if (deck == null)
        {
            Debug.LogError("DeckScriptableObject not found, cannot add card to hand");
            return;
        }

        PlayerCard newCard = new PlayerCard(deck.GetNextCard());
        currentHand.Add(newCard);
    }

    public PlayerCard GetLastCardInHand()
    {
        if (currentHand.Count == 0) return null;

        return currentHand[currentHand.Count - 1];
    }
}
