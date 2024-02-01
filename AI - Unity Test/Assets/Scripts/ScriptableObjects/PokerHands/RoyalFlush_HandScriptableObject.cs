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

        //check for straight flush, cache the result
        bool isStraightFlush = straightFlush.IsHandValid(true);
        //check for Ace and Ten
        bool hasAce = playerHand.GetCurrentHand.Exists(x => x.cardValue == CardValue.Ace);
        bool hasTen = playerHand.GetCurrentHand.Exists(x => x.cardValue == CardValue.Ten);

        return isStraightFlush && hasAce && hasTen;
    }
}
