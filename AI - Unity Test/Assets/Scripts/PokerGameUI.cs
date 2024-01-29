using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokerGameUI : MonoBehaviour
{
    [SerializeField] private List<CardButtonUI> playerHandUI = new List<CardButtonUI>();
    [SerializeField] private Button messageBanner = null;
       
    [SerializeField] private TextMeshProUGUI currentHandText = null;

    public void InitializeUI(PayoutTableScriptableObject payoutTable)
    {
        //if (dealButton != null) dealButton.onClick.AddListener(OnDrawButtonClicked);

        if (currentHandText != null) currentHandText.text = string.Empty;        
    }

    //public void OnDrawButtonClicked()
    //{
    //    if (currentGameState == PokerGameState.Deal)
    //    {
    //        if (playerHand == null)
    //        {
    //            Debug.LogError("Player Hand ScriptableObject not set, cannot start game");
    //            return;
    //        }

    //        currentHandText.text = string.Empty;

    //        playerHand.InitializeHand();

    //        if (handEvaluator[0].IsHandValid()) Debug.LogError("Is Valid Hand");
    //        else Debug.LogError("Not a Valid Hand");
    //        return;

    //        if (playerHand.GetCurrentHand.Count != playerHandUI.Count)
    //        {
    //            Debug.LogError("Player Card Count does not match Card UI Count, cannot start game");
    //            return;
    //        }

    //        if (messageBanner != null) messageBanner.gameObject.SetActive(false);

    //        for (int i = 0; i < playerHand.GetCurrentHand.Count; i++)
    //        {
    //            playerHandUI[i].UpdateUI(playerHand.GetCurrentHand[i].cardSprite);
    //        }

    //        if (dealButtonText != null) dealButtonText.text = "DRAW";

    //        currentGameState = PokerGameState.ToDraw;

    //        for (int i = 0; i < payoutTable.GetPayoutTableList.Count; i++)
    //        {
    //            if (payoutTable.GetPayoutTableList[i].GetPokerHandSO.IsHandValid())
    //            {
    //                currentHandText.text = payoutTable.GetPayoutTableList[i].GetPokerHandSO.GetDisplayName;
    //                return;
    //            }
    //        }

    //        return;
    //    }

    //    if (currentGameState == PokerGameState.ToDraw)
    //    {
    //        for (int i = 0; i < playerHandUI.Count; i++)
    //        {
    //            if (playerHandUI[i].GetHeldState)
    //            {
    //                playerHandUI[i].OnCardClicked();
    //                continue;
    //            }

    //            playerHand.ReplaceCardFromDeck(i);
    //            playerHandUI[i].UpdateUI(playerHand.GetCurrentHand[i].cardSprite);
    //        }

    //        if (dealButtonText != null) dealButtonText.text = "DEAL";

    //        currentGameState = PokerGameState.Deal;

    //        for (int i = 0; i < payoutTable.GetPayoutTableList.Count; i++)
    //        {
    //            if (payoutTable.GetPayoutTableList[i].GetPokerHandSO.IsHandValid())
    //            {
    //                if (currentHandText != null) currentHandText.text = payoutTable.GetPayoutTableList[i].GetPokerHandSO.GetDisplayName;
    //                return;
    //            }
    //        }
    //    }
    //}
}
