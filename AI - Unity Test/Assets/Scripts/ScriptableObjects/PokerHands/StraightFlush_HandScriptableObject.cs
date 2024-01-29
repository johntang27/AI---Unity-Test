using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StraightFlush Hand", menuName = "ScriptableObjects/PokerHands/StraightFlush")]
public class StraightFlush_HandScriptableObject : PokerHandScriptableObject
{
    [SerializeField] private Straight_HandScriptableObject straight = null;
    [SerializeField] private Flush_HandScriptableObject flush = null;

    private bool result = false;

    public override bool IsHandValid(bool toCacheResult = false)
    {
        if (playerHand == null || straight == null || flush == null) return false;

        if(!string.IsNullOrEmpty(cachedResult))
        {
            bool isValidResult = bool.TryParse(cachedResult, out result);
            if(isValidResult)
            {
                cachedResult = string.Empty;
                return result;
            }
        }

        bool isStraight = straight.IsHandValid(true);
        bool isFlush = flush.IsHandValid(true);

        result = isStraight && isFlush;
        if (toCacheResult) cachedResult = result.ToString();
        return result;
    }
}
