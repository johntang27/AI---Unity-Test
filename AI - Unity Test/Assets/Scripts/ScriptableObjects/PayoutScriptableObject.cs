using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PayoutScriptableObject", menuName = "ScriptableObjects/PayoutScriptableObject")]
public class PayoutScriptableObject : ScriptableObject
{
    [SerializeField] private PokerHandScriptableObject pokerHandSO;
    [SerializeField] private int basePayout;

    public PokerHandScriptableObject GetPokerHandSO => pokerHandSO;
    public int GetBasePayout => basePayout;
}
