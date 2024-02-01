using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Straight Hand", menuName = "ScriptableObjects/PokerHands/Straight")]
public class Straight_HandScriptableObject : PokerHandScriptableObject
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

        //return an ordered list based on cardValue
        var ordered = playerHand.GetCurrentHand.OrderBy(card => card.cardValue).ToList(); 
        int consecutive = 0;
        for (int i = 0; i < ordered.Count - 1; i++)
        {
            //Debug.LogError(ordered[i].cardValue);
            if ((int)ordered[i].cardValue + 1 != (int)ordered[i + 1].cardValue)
            {
                //Ace can also be treated as the lowest rank to complete a straight (e.g. A-2-3-4-5), in my ordered enum, Ace is considered the highest.
                //This is a special edge case check to see if the first ordered card is 2 and also if the last card is Ace
                //The consecutive variable is a counter to track if 2-3-4-5 has happened
                if (consecutive == 3 && ordered[i + 1].cardValue == CardValue.Ace && ordered[0].cardValue == CardValue.Two)
                    result = true;
                else
                    result = false;

                if (toCacheResult) cachedResult = result.ToString();
                return result;
            }
            consecutive++;
        }

        result = true;
        if (toCacheResult) cachedResult = result.ToString();
        return result;
    }
}
