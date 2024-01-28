using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PayoutTableScriptableTable", menuName = "ScriptableObjects/PayoutTableScriptableTable")]
public class PayoutTableScriptableObject : ScriptableObject
{
    [Tooltip("Payout Order from Highest to Lowest")]
    [SerializeField] private List<PayoutScriptableObject> payoutTableList = new List<PayoutScriptableObject>();
    [SerializeField] private List<int> betTierMultipliers = new List<int>();

    public List<PayoutScriptableObject> GetPayoutTableList => payoutTableList;
    public List<int> GetBetTierMultipliers => betTierMultipliers;
}
