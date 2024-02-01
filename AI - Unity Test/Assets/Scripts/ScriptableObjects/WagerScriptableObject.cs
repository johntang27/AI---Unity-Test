using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles bet logic for the game, currently only used by Blackjack, which it inherits from
/// Reference the PlayerDataScriptableObject, so we can directly change player's credits
/// </summary>
public class WagerScriptableObject : ScriptableObject
{
    [SerializeField] protected PlayerDataScriptableObject playerData = null;
    [SerializeField] protected int currentBet;
    [SerializeField] protected int minBet;
    [SerializeField] protected int maxBet;
    [SerializeField] protected int betIncrement;

    protected int winning = 0;
    public int GetWinning => winning;

    public int GetCurrentBet => currentBet;
    //public int GetMinBet => minBet;
    //public int GetMaxBet => maxBet;
    //public int GetBetIncrement => betIncrement;
    public bool IsCurrentBetMin => currentBet == minBet;
    public bool IsCurrentBetMax => currentBet == maxBet;
    public bool CanIncreaseBet => playerData.PlayerCredits >= currentBet + betIncrement;
    public bool CanPlaceBet => playerData.PlayerCredits >= currentBet;

    public void IncreaseBet()
    {
        if (currentBet <= maxBet) currentBet += betIncrement;
    }

    public void DecreaseBet()
    {
        if (currentBet >= minBet) currentBet -= betIncrement;
    }

    public void PlaceBet()
    {
        playerData.PlayerCredits -= currentBet;
    }

    public void ValidateBet()
    {
        if (currentBet < minBet) currentBet = minBet;
        if (currentBet > maxBet) currentBet = maxBet;
    }

    public virtual void DoubleDownBet() { }

    public virtual void AwardWinning() { }

    public virtual void AwardWinning(BlackjackResult result) { }

}
