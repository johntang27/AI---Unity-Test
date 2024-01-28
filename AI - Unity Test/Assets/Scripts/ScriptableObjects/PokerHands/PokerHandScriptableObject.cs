using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerHandScriptableObject : ScriptableObject
{
    [SerializeField] protected PlayerHandScriptableObject playerHand = null;
    [SerializeField] protected string pokerHandDisplayName;
    [SerializeField] protected HandRanking handRanking = HandRanking.NoHand;

    [SerializeField] protected string cachedResult = string.Empty;

    public string GetDisplayName => pokerHandDisplayName;
    public string GetCachedResult => cachedResult;

    public virtual bool IsHandValid(bool toCacheResult = false)
    {
        return false;
    }

    public void ResetCachedResult()
    {
        cachedResult = string.Empty;
    }
}
