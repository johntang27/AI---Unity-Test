using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game Setting for a standard Blackjack game
/// With some specific setting for Blackjack, such as the blackjackGoal, can be changed to some other number, if we want to be creative, instead of 21
/// Its own wager ScriptableObject to handle the bets
/// Its own HandScriptableObject for both the player and dealer
/// </summary>
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
