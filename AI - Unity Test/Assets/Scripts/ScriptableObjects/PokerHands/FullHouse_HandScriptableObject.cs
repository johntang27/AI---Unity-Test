using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FullHouse Hand", menuName = "ScriptableObjects/PokerHands/FullHouse")]
public class FullHouse_HandScriptableObject : PokerHandScriptableObject
{
    [SerializeField] private ThreeOfAKind_HandScriptableObject threeOfAKind = null;

    public override bool IsHandValid(bool toCacheResult = false)
    {
        if (playerHand == null || threeOfAKind == null) return false;

        //calculate 3 of a kind, then cache the result in case it's not a full house
        bool isThree = threeOfAKind.IsHandValid(true);
        //check for a pair
        bool isPair = playerHand.GetCurrentHand.GroupBy(card => card.cardValue).Where(group => group.Count() == 2).Count() == 1;

        return isThree & isPair;
    }
}
