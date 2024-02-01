using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScriptableObject", menuName = "ScriptableObjects/CardScriptableObject")]
public class CardScriptableObject : ScriptableObject
{
    [SerializeField] private CardSpriteScriptableObject cardSpriteSourceSO;
    [SerializeField] private string cardName;
    [SerializeField] private CardSuit cardSuit;
    [SerializeField] private CardValue cardValue;
    [SerializeField] private Sprite cardSprite;
    [SerializeField] private int blackjackCardValue;

    public CardSpriteScriptableObject GetCardSpriteSourceSO => cardSpriteSourceSO;
    public CardSuit GetSuit => cardSuit;
    public CardValue GetValue => cardValue;
    public Sprite GetSprite => cardSprite;
    public int GetBlackjackCardValue => blackjackCardValue;

/// <summary>
/// Sets up the card data procedurally by using the name of the sprite
/// </summary>
/// <param name="sprite">the actual sprite to display the card</param>
/// <param name="spriteSourceSO">CardSpriteScriptableObject contains the location of the card spriteSheet, 
/// so it can be used if you need to update a specific single card</param>
    public void UpdateData(Sprite sprite, CardSpriteScriptableObject spriteSourceSO = null)
    {
        string[] splits = this.name.Split('_');

        cardName = this.name;

        if (splits.Length == 1) return; //joker card

        bool canGetValue = System.Enum.TryParse(splits[0], out cardValue);
        if (!canGetValue) Debug.LogErrorFormat("cannot parse {0} card value from name", splits[0]);

        bool canGetSuit = System.Enum.TryParse(splits[1], out cardSuit);
        if (!canGetSuit) Debug.LogErrorFormat("cannot parse {0} card suit from name", splits[1]);

        if (sprite != null) cardSprite = sprite;
        else Debug.LogErrorFormat("cannot find sprite for {0}", cardName);

        if (spriteSourceSO != null) cardSpriteSourceSO = spriteSourceSO;

        if ((int)cardValue < (int)CardValue.Jack) blackjackCardValue = (int)cardValue + 2;
        if (cardValue == CardValue.Jack || cardValue == CardValue.Queen || cardValue == CardValue.King) blackjackCardValue = 10;
        if (cardValue == CardValue.Ace) blackjackCardValue = 11;
    }
}
