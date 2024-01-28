using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FourOfAKind Hand", menuName = "ScriptableObjects/PokerHands/FourOfAKind")]
public class FourOfAKind_HandScriptableObject : PokerHandScriptableObject
{
    public override bool IsHandValid(bool toCacheResult = false)
    {
        if (playerHand == null) return false;

        return playerHand.GetCurrentHand.GroupBy(card => card.cardValue).Where(group => group.Count() == 4).Any();
    }
}
