using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackjackSetting", menuName = "ScriptableObjects/GameSetting/BlackjackSetting")]
public class BlackjackSettingScriptableObject : GameModeSettingScriptableObject
{
    [SerializeField] private BlackjackHandScriptableObject playerHand = null;
    [SerializeField] private BlackjackHandScriptableObject dealerHand = null;

    public BlackjackHandScriptableObject GetPlayerHand => playerHand;
    public BlackjackHandScriptableObject GetDealerHand => dealerHand;
}
