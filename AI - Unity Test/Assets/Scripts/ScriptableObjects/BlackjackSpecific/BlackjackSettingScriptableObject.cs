using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackjackSetting", menuName = "ScriptableObjects/GameSetting/BlackjackSetting")]
public class BlackjackSettingScriptableObject : GameModeSettingScriptableObject
{
    [SerializeField] private int blackjackGoal;
    [SerializeField] private WagerScriptableObject wager = null;
    [SerializeField] private BlackjackHandScriptableObject playerHand = null;
    [SerializeField] private BlackjackHandScriptableObject dealerHand = null;

    public int GetBlackjackGoal => blackjackGoal;
    public WagerScriptableObject GetWager => wager;
    public BlackjackHandScriptableObject GetPlayerHand => playerHand;
    public BlackjackHandScriptableObject GetDealerHand => dealerHand;
}
