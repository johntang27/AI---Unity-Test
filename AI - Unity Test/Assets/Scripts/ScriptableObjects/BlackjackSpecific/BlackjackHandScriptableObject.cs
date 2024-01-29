using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackjackHandScriptableObject", menuName = "ScriptableObjects/BlackjackHandScriptableObject")]
public class BlackjackHandScriptableObject : PlayerHandScriptableObject
{
    public override void InitializeHand(int handSize)
    {
        if (deck != null) deck.ShuffleDeck();
        currentHand = new List<PlayerCard>();
    }

    public void DrawCardFromDeck(int cardIndex)
    {
        if (deck == null)
        {
            Debug.LogError("DeckScriptableObject not found, cannot add card to hand");
            return;
        }

        if (cardIndex < deck.GetDeck.Count)
        {
            PlayerCard newCard = new PlayerCard(deck.GetDeck[cardIndex]);
            currentHand.Add(newCard);
        }        
    }
}
