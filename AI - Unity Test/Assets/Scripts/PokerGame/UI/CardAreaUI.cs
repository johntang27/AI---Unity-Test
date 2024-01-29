using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardAreaUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentHandText = null;
    [SerializeField] private CardButtonUI cardButtonUI = null;
    [SerializeField] private Transform cardAreaContainer = null;    
    [SerializeField] private GameObject messageBanner = null;
    [SerializeField] private TextMeshProUGUI messageBannerText = null;

    private List<CardButtonUI> playerHandUI = new List<CardButtonUI>();
    public List<CardButtonUI> GetPlayerHandUI => playerHandUI;

    public void Init()
    {
        if (currentHandText != null) currentHandText.text = string.Empty;

        if (cardAreaContainer != null && cardButtonUI != null)
        {
            for (int i = 0; i < PokerGameManager.Instance.GetGameSetting.GetPlayerStartingHandSize; i++)
            {
                CardButtonUI cardButton = Instantiate(cardButtonUI);
                cardButton.transform.SetParent(cardAreaContainer);
                playerHandUI.Add(cardButton);
            }
        }

        if (messageBanner != null)
        {
            messageBanner.SetActive(true);
            messageBanner.transform.SetAsLastSibling();
        }
    }

    public void UpdateMessageBannerText()
    {
        messageBanner.SetActive(true);
        messageBannerText.text = string.Format("PLAY {0} CREDITS", PokerGameManager.Instance.GetCurrentBet);
    }

    public void UpdateCurrentValidHandText(string handName)
    {
        currentHandText.text = handName;
    }

    public void UpdateStartingHandUI()
    {
        if (PokerGameManager.Instance.GetGameSetting.GetPlayerHand == null)
        {
            Debug.LogError("Player Hand ScriptableObject not set, cannot start game");
            return;
        }

        if (PokerGameManager.Instance.GetGameSetting.GetPlayerHand.GetCurrentHand.Count != playerHandUI.Count)
        {
            Debug.LogError("Player Card Count does not match Card UI Count, cannot start game");
            return;
        }

        currentHandText.text = string.Empty;
        messageBanner.SetActive(false);

        for (int i = 0; i < PokerGameManager.Instance.GetGameSetting.GetPlayerHand.GetCurrentHand.Count; i++)
        {
            playerHandUI[i].ToggleInteraction(true);
            playerHandUI[i].UpdateUI(PokerGameManager.Instance.GetGameSetting.GetPlayerHand.GetCurrentHand[i].cardSprite);
        }
    }

    public void UpdateNewHandUI()
    {
        for (int i = 0; i < playerHandUI.Count; i++)
        {
            playerHandUI[i].ToggleInteraction(false);

            if (playerHandUI[i].GetHeldState)
            {
                playerHandUI[i].OnCardClicked();
                continue;
            }

            PokerGameManager.Instance.GetGameSetting.GetPlayerHand.ReplaceCardFromDeck(i);
            playerHandUI[i].UpdateUI(PokerGameManager.Instance.GetGameSetting.GetPlayerHand.GetCurrentHand[i].cardSprite);
        }
    }
}
