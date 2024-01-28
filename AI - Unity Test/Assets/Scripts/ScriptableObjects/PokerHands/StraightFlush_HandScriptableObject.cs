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

        bool isStraight = straight.IsHandValid(true);
        bool isFlush = flush.IsHandValid(true);

        result = isStraight && isFlush;
        if (toCacheResult) cachedResult = result.ToString();
        return result;
    }
}
