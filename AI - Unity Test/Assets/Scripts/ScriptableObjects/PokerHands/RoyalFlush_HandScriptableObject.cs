using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoyalFlush Hand", menuName = "ScriptableObjects/PokerHands/RoyalFlush")]
public class RoyalFlush_HandScriptableObject : PokerHandScriptableObject
{
    [SerializeField] private StraightFlush_HandScriptableObject straightFlush = null;

    public override bool IsHandValid(bool toCacheResult = false)
    {
        if (playerHand == null || straightFlush == null) return false;

        bool isStraightFlush = straightFlush.IsHandValid(true);
        bool hasAceAndTen = playerHand.GetCurrentHand.Any(card => card.cardValue == CardValue.Ace || card.cardValue == CardValue.Ten);

        return isStraightFlush && hasAceAndTen;
    }
}
