using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckScriptableObject", menuName = "ScriptableObjects/DeckScriptableObject")]
public class DeckScriptableObject : ScriptableObject
{
    [SerializeField] private List<CardScriptableObject> cards;
    [SerializeField] private bool canShuffle = true;

    [SerializeField] private int nextCardIndex = 0;
    public List<CardScriptableObject> GetDeck => cards;

    public void ShuffleDeck()
    {
        if (!canShuffle) return;

        if (cards.Count > 0)
        {
            //Debug.LogError("To shuffle");
            for (int i = 0; i < cards.Count; i++)
            {
                int rand = Random.Range(0, cards.Count);
                CardScriptableObject temp = cards[i];
                cards[i] = cards[rand];
                cards[rand] = temp;
            }
        }
        nextCardIndex = 0;
    }

    public CardScriptableObject GetNextCard()
    {
        if (nextCardIndex >= cards.Count)
        {
            nextCardIndex = 0;
        }

        CardScriptableObject nextCard = cards[nextCardIndex];
        nextCardIndex++;

        return nextCard;
    }
}
