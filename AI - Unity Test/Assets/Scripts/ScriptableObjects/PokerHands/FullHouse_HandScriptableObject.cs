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

        bool isThree = threeOfAKind.IsHandValid(true);
        bool isPair = playerHand.GetCurrentHand.GroupBy(card => card.cardValue).Where(group => group.Count() == 2).Count() == 1;

        return isThree & isPair;
    }
}
