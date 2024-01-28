using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TwoPairs Hand", menuName = "ScriptableObjects/PokerHands/TwoPairs")]
public class TwoPairs_HandScriptableObject : PokerHandScriptableObject 
{
    public override bool IsHandValid(bool toCacheResult = false)
    {
        if (playerHand == null) return false;

        return playerHand.GetCurrentHand.GroupBy(card => card.cardValue).Where(group => group.Count() == 2).Count() == 2;
    }
}
