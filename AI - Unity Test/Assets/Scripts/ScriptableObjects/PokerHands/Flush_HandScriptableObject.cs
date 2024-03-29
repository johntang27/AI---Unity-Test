using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flush Hand", menuName = "ScriptableObjects/PokerHands/Flush")]
public class Flush_HandScriptableObject : PokerHandScriptableObject
{
    private bool result = false;

    public override bool IsHandValid(bool toCacheResult = false)
    {
        if (playerHand == null) return false;

        if (!string.IsNullOrEmpty(cachedResult)) //check if there's a cached result from checking another combination, ex. straight flush
        {
            bool isValidResult = bool.TryParse(cachedResult, out result);
            if (isValidResult) //return the cached result instead if parsing is succesfull
            {
                cachedResult = string.Empty; 
                return result;
            }
        }

        result = playerHand.GetCurrentHand.GroupBy(card => card.cardSuit).Count() == 1;

        if (toCacheResult) cachedResult = result.ToString();

        return result;
    }
}
