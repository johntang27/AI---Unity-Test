using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Common variables that will be inherited by all child ScriptableObjects to define the hand
/// A virtual IsHandValid method to be override by child with its own corrsponding logic
/// </summary>
public class PokerHandScriptableObject : ScriptableObject
{
    [SerializeField] protected PlayerHandScriptableObject playerHand = null;
    [SerializeField] protected string pokerHandDisplayName;

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
