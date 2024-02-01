using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The PayoutScriptableObject is responsible for referring to the poker hand and its base payout amount
/// </summary>
[CreateAssetMenu(fileName = "PayoutScriptableObject", menuName = "ScriptableObjects/PayoutScriptableObject")]
public class PayoutScriptableObject : ScriptableObject
{
    [SerializeField] private PokerHandScriptableObject pokerHandSO;
    [SerializeField] private int basePayout;

    public PokerHandScriptableObject GetPokerHandSO => pokerHandSO;
    public int GetBasePayout => basePayout;
}
