using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains specific Poker game setting properties
/// Identitfy what type of poker game it is
/// Its own PlayerHandScriptableObject, and PayoutTableScriptableObject for this specific poker game
/// </summary>
[CreateAssetMenu(fileName = "PokerGameSetting", menuName = "ScriptableObjects/GameSetting/PokerGameSetting")]
public class PokerGameSettingScriptableObject : GameModeSettingScriptableObject
{
    [SerializeField] private PokerSubGame pokerSubGame;
    [SerializeField] private PlayerHandScriptableObject playerHand = null;
    [SerializeField] private PayoutTableScriptableObject payoutTable = null;

    public PokerSubGame GetPokerSubGame => pokerSubGame;
    public PlayerHandScriptableObject GetPlayerHand => playerHand;
    public PayoutTableScriptableObject GetPayoutTable => payoutTable;
}
