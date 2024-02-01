using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokerGameManager : Singleton<PokerGameManager>
{
    [SerializeField] private PokerGameSettingScriptableObject pokerGameSetting = null;
    [SerializeField] private PayoutTableUI payoutTableUI = null;
    [SerializeField] private CardAreaUI cardAreaUI = null;
    [SerializeField] private BottomGameAreaUI bottomGameAreaUI = null;

    private PokerGameState currentGameState = PokerGameState.Deal;
    public PokerGameState GetCurrentGameState => currentGameState;

    private int currentBet = 1;
    public int GetCurrentBet => currentBet;

    private int currentPayoutMultiplierIndex = 0;
    public int GetCurrentPayoutMultiplierIndex => currentPayoutMultiplierIndex;

    public PokerGameSettingScriptableObject GetGameSetting => pokerGameSetting;
    public int GetPlayerCredits => pokerGameSetting.GetPlayerData.PlayerCredits;

    //used for controlling the game speed
    private float currentNextDealDelay = 2f;
    private float maxNextDealDelay = 2f;
    private float minNextDealDelay = 1f;
    private float delayDecrement = 0.5f;
    private int speedIndex = 0;

    void Start()
    {
        if (pokerGameSetting == null)
        {
            if (MainGameController.Instance != null)
            {
                pokerGameSetting = (PokerGameSettingScriptableObject)MainGameController.Instance.settingsToLoad;
            }
        }

        //update all the UI after game setting is set
        if (pokerGameSetting != null)
        {
            if (payoutTableUI != null) payoutTableUI.Init();
            if (cardAreaUI != null) cardAreaUI.Init();
            if (bottomGameAreaUI != null) bottomGameAreaUI.Init();
        }
    }
    #region PRIVATE METHODS
    private void DealCards()
    {
        if (pokerGameSetting.GetPlayerHand == null)
        {
            Debug.LogError("Player Hand ScriptableObject not set, cannot start game");
            return;
        }

        payoutTableUI.DisableFlashingPayoutContent(); //turn off any flashing effect on the payout table after a winning hand

        pokerGameSetting.GetPlayerHand.InitializeHand(pokerGameSetting.GetPlayerStartingHandSize);

        cardAreaUI.UpdateStartingHandUI();

        pokerGameSetting.GetPlayerData.PlayerCredits -= currentBet;

        bottomGameAreaUI.UpdateUIAfterDeal();

        for (int i = 0; i < pokerGameSetting.GetPayoutTable.GetPayoutTableList.Count; i++)
        {
            //go through all the possible hand logic check until we find one, then display the current hand name
            if (pokerGameSetting.GetPayoutTable.GetPayoutTableList[i].GetPokerHandSO.IsHandValid())
            {
                cardAreaUI.UpdateCurrentValidHandText(pokerGameSetting.GetPayoutTable.GetPayoutTableList[i].GetPokerHandSO.GetDisplayName);
                break;
            }
        }

        currentGameState = PokerGameState.Draw;
    }

    private void DrawCards()
    {
        cardAreaUI.UpdateNewHandUI(); //update and replace any non-held cards UI

        for (int i = 0; i < pokerGameSetting.GetPayoutTable.GetPayoutTableList.Count; i++)
        {
            //go through all the possible hand logic check until we find one 
            if (pokerGameSetting.GetPayoutTable.GetPayoutTableList[i].GetPokerHandSO.IsHandValid())
            {
                cardAreaUI.UpdateCurrentValidHandText(pokerGameSetting.GetPayoutTable.GetPayoutTableList[i].GetPokerHandSO.GetDisplayName);
                //determine how much the player has won
                int winning = pokerGameSetting.GetPayoutTable.GetPayoutTableList[i].GetBasePayout * pokerGameSetting.GetPayoutTable.GetBetTierMultipliers[currentPayoutMultiplierIndex];
                pokerGameSetting.GetPlayerData.PlayerCredits += winning;
                //show winning UI
                bottomGameAreaUI.ShowWinText(winning);
                payoutTableUI.FlashWinningPayout(i, currentPayoutMultiplierIndex + 1);
                break;
            }
        }

        currentGameState = PokerGameState.Deal;

        StartCoroutine(DelayNextDeal());
    }

    //reset the UI
    IEnumerator DelayNextDeal()
    {
        yield return new WaitForSeconds(currentNextDealDelay);
        cardAreaUI.UpdateMessageBannerText();
        bottomGameAreaUI.UpdateUIAfterResult();
        bottomGameAreaUI.ToggleBetButtons(currentBet > 1, currentBet < pokerGameSetting.GetPayoutTable.GetBetTierMultipliers.Count);
    }
    #endregion

    #region PUBLIC METHODS
    public void ChangeCurrentBet(int change)
    {
        //increasing the bet
        if (currentBet < pokerGameSetting.GetPayoutTable.GetBetTierMultipliers.Count && change > 0)
        {
            //turn off the previous highlight
            payoutTableUI.HighlightPayoutTier(false);
            //make the changes
            currentBet += change;
            currentPayoutMultiplierIndex++;
            //turn on the highlight of the new column
            payoutTableUI.HighlightPayoutTier(true);
        }
        //decreasing the bet
        else if (currentBet > 1 && change < 0)
        {
            //turn off the previous highlight
            payoutTableUI.HighlightPayoutTier(false);
            //make the changes
            currentBet += change;
            currentPayoutMultiplierIndex--;
            //turn on the highlight of the new column
            payoutTableUI.HighlightPayoutTier(true);
        }

        cardAreaUI.UpdateMessageBannerText();

        bottomGameAreaUI.ToggleBetButtons(currentBet > 1, currentBet < pokerGameSetting.GetPayoutTable.GetBetTierMultipliers.Count);
    }

    public void DealOrDrawButtonClicked()
    {
        if (currentGameState == PokerGameState.Deal)
            DealCards();
        else if (currentGameState == PokerGameState.Draw)
            DrawCards();
    }

    //return the new speed reference
    public int AdjustSpeed()
    {
        if (currentNextDealDelay > minNextDealDelay)
        {
            currentNextDealDelay -= delayDecrement;
            speedIndex++;
        }
        else //when we are at the fastest delay, we switch it back down to the slowest
        {
            currentNextDealDelay = maxNextDealDelay;
            speedIndex = 0;
        }

        return speedIndex;
    }
    #endregion
}
