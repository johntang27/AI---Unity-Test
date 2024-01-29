using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BottomGameAreaUI : MonoBehaviour
{
    private const string CURRENT_BET_STRING = "BET: {0}";
    private const string TOTAL_CREDITS_STRING = "TOTAL CREDITS: {0}";
    private const string WIN_AMOUNT_STRING = "WIN {0}";

    [SerializeField] private TextMeshProUGUI winAmountText = null;
    [SerializeField] private TextMeshProUGUI currentBetText = null;
    [SerializeField] private TextMeshProUGUI availableCreditsText = null;
    [SerializeField] private Button gameInfoButton = null;
    [SerializeField] private Button moreGamesButton = null;
    [SerializeField] private Button speedUpButton = null;
    [SerializeField] private Button betDownButton = null;
    [SerializeField] private Button betUpButton = null;
    [SerializeField] private Button dealButton = null;
    [SerializeField] private TextMeshProUGUI dealButtonText = null;

    public void Init()
    {
        if (PokerGameManager.Instance == null)
        {
            Debug.LogError("PokerGameManager not found, cannot update UI");
            return;
        }

        if (winAmountText != null) winAmountText.gameObject.SetActive(false);
        UpdateCurrentBetText();
        UpdateTotalCreditsText();

        if (gameInfoButton != null) gameInfoButton.onClick.AddListener(OnGameInfoClicked);
        if (betDownButton != null) betDownButton.onClick.AddListener(OnBetDownClicked);
        if (betUpButton != null) betUpButton.onClick.AddListener(OnBetUpClicked);
        if (dealButton != null) dealButton.onClick.AddListener(OnDealClicked);
    }

    private void UpdateCurrentBetText()
    {
        if (currentBetText != null) currentBetText.text = string.Format(CURRENT_BET_STRING, PokerGameManager.Instance.GetCurrentBet);
    }

    private void UpdateTotalCreditsText()
    {
        if (availableCreditsText != null) availableCreditsText.text = string.Format(TOTAL_CREDITS_STRING, PokerGameManager.Instance.GetPlayerCredits);
    }

    private void UpdateDealButtonText(string buttonText)
    {
        if (dealButtonText != null) dealButtonText.text = buttonText;
    }

    private void OnGameInfoClicked()
    {
        Application.OpenURL(PokerGameManager.Instance.GetGameSetting.GetGameInfoUrl);
    }

    private void OnBetDownClicked()
    {
        PokerGameManager.Instance.ChangeCurrentBet(-1);
        currentBetText.text = string.Format(CURRENT_BET_STRING, PokerGameManager.Instance.GetCurrentBet);
    }

    private void OnBetUpClicked()
    {
        PokerGameManager.Instance.ChangeCurrentBet(1);
        currentBetText.text = string.Format(CURRENT_BET_STRING, PokerGameManager.Instance.GetCurrentBet);
    }

    private void OnDealClicked()
    {
        PokerGameManager.Instance.DealOrDrawButtonClicked();
    }

    public void ToggleBetButtons(bool canGoDown, bool canGoUp)
    {
        betDownButton.interactable = canGoDown;
        betUpButton.interactable = canGoUp;
    }

    public void UpdateUIAfterDeal()
    {
        UpdateDealButtonText("DRAW");
        UpdateCurrentBetText();
        UpdateTotalCreditsText();
        moreGamesButton.interactable = false;
        betDownButton.interactable = false;
        betUpButton.interactable = false;
    }

    public void ShowWinText(int amt)
    {
        winAmountText.gameObject.SetActive(true);
        winAmountText.text = string.Format(WIN_AMOUNT_STRING, amt);
        UpdateTotalCreditsText();
    }

    public void UpdateUIAfterResult()
    {
        winAmountText.gameObject.SetActive(false);
        UpdateDealButtonText("DEAL");
        moreGamesButton.interactable = true;
    }
}
