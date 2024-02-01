using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThreeOfAKind Hand", menuName = "ScriptableObjects/PokerHands/ThreeOfAKind")]
public class ThreeOfAKind_HandScriptableObject : PokerHandScriptableObject
{
    private bool result = false;

    public override bool IsHandValid(bool toCacheResult = false)
    {
        if (playerHand == null) return false;

        if (!string.IsNullOrEmpty(cachedResult)) //check if there's a cached result from checking another combination, ex. full house
        {
            bool isValidResult = bool.TryParse(cachedResult, out result);
            if (isValidResult)
            {
                cachedResult = string.Empty;
                return result;
            }
        }

        result = playerHand.GetCurrentHand.GroupBy(card => card.cardValue).Where(group => group.Count() == 3).Any();

        if (toCacheResult) cachedResult = result.ToString();

        return result;
    }
}
