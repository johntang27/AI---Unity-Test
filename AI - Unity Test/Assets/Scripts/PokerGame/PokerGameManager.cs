using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokerGameManager : Singleton<PokerGameManager>
{
    [SerializeField] private PokerGameSettingScriptableObject pokerGameSetting;

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

    private int playerCredits
    {
        get => PlayerPrefs.GetInt("PlayerCredits", 100);
        set => PlayerPrefs.SetInt("PlayerCredits", value);
    }
    public int GetPlayerCredits => playerCredits;


    void Start()
    {
        if (pokerGameSetting == null)
        {
            if (MainGameController.Instance != null)
            {
                pokerGameSetting = (PokerGameSettingScriptableObject)MainGameController.Instance.settingsToLoad;
            }
        }

        if (pokerGameSetting != null)
        {
            if (payoutTableUI != null) payoutTableUI.Init();
            if (cardAreaUI != null) cardAreaUI.Init();
            if (bottomGameAreaUI != null) bottomGameAreaUI.Init();
        }
    }

    private void DealCards()
    {
        if (pokerGameSetting.GetPlayerHand == null)
        {
            Debug.LogError("Player Hand ScriptableObject not set, cannot start game");
            return;
        }

        payoutTableUI.DisableFlashingPayoutContent();

        pokerGameSetting.GetPlayerHand.InitializeHand(pokerGameSetting.GetPlayerStartingHandSize);

        cardAreaUI.UpdateStartingHandUI();

        playerCredits -= currentBet;

        bottomGameAreaUI.UpdateUIAfterDeal();

        for (int i = 0; i < pokerGameSetting.GetPayoutTable.GetPayoutTableList.Count; i++)
        {
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
        cardAreaUI.UpdateNewHandUI();

        for (int i = 0; i < pokerGameSetting.GetPayoutTable.GetPayoutTableList.Count; i++)
        {
            if (pokerGameSetting.GetPayoutTable.GetPayoutTableList[i].GetPokerHandSO.IsHandValid())
            {
                cardAreaUI.UpdateCurrentValidHandText(pokerGameSetting.GetPayoutTable.GetPayoutTableList[i].GetPokerHandSO.GetDisplayName);
                int winning = pokerGameSetting.GetPayoutTable.GetPayoutTableList[i].GetBasePayout * pokerGameSetting.GetPayoutTable.GetBetTierMultipliers[currentPayoutMultiplierIndex];
                playerCredits += winning;
                bottomGameAreaUI.ShowWinText(winning);
                payoutTableUI.FlashWinningPayout(i, currentPayoutMultiplierIndex + 1);
                break;
            }
        }

        currentGameState = PokerGameState.Deal;

        StartCoroutine(DelayNextDeal());
    }

    IEnumerator DelayNextDeal()
    {
        yield return new WaitForSeconds(2f);
        cardAreaUI.UpdateMessageBannerText();
        bottomGameAreaUI.UpdateUIAfterResult();
        bottomGameAreaUI.ToggleBetButtons(currentBet > 1, currentBet < pokerGameSetting.GetPayoutTable.GetBetTierMultipliers.Count);
    }

    public void ChangeCurrentBet(int change)
    {
        if (currentBet < pokerGameSetting.GetPayoutTable.GetBetTierMultipliers.Count && change > 0)
        {
            payoutTableUI.HighlightPayoutTier(false);
            currentBet += change;
            currentPayoutMultiplierIndex++;
            payoutTableUI.HighlightPayoutTier(true);
        }
        else if (currentBet > 1 && change < 0)
        {
            payoutTableUI.HighlightPayoutTier(false);
            currentBet += change;
            currentPayoutMultiplierIndex--;
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
}
