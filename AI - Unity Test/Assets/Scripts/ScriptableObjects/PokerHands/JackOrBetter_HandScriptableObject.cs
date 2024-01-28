using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JackOrBetter Hand", menuName = "ScriptableObjects/PokerHands/JackOrBetter")]
public class JackOrBetter_HandScriptableObject : PokerHandScriptableObject
{
    public override bool IsHandValid(bool toCacheResult = false)
    {
        if (playerHand == null) return false;

        return playerHand.GetCurrentHand.GroupBy(card => card.cardValue).Where(group => group.Count() == 2 && (int)group.Key > (int)CardValue.Ten).Count() == 1;
    }
}
