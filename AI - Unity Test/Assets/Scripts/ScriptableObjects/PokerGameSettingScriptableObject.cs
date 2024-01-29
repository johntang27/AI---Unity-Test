using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokerGameSetting", menuName = "ScriptableObjects/GameSetting/PokerGameSetting")]
public class PokerGameSettingScriptableObject : GameModeSettingScriptableObject
{
    [SerializeField] private PokerSubGame pokerSubGame;
    [SerializeField] private int playerHandCount;
    [SerializeField] private string gameInfoUrl;
    [SerializeField] private PlayerHandScriptableObject playerHand = null;
    [SerializeField] private PayoutTableScriptableObject payoutTable = null;

    public PokerSubGame GetPokerSubGame => pokerSubGame;
    public int GetPlayerHandCount => playerHandCount;
    public string GetGameInfoUrl => gameInfoUrl;
    public PlayerHandScriptableObject GetPlayerHand => playerHand;
    public PayoutTableScriptableObject GetPayoutTable => payoutTable;
}
