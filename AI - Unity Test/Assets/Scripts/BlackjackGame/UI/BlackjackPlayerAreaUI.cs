using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BlackjackPlayerAreaUI : BlackjackCardAreaUI
{
    #region CONSTANTS
    private const string CREDITS_TEXT_PREFIX = "AVAILABLE CREDITS: {0}";
    private const string WIN_BANNER_TEXT = "You Win!\n{0} Credits";
    private const string PUSH_BANNER_TEXT = "PUSH, {0} Credits Back";
    #endregion

    [Header("Player Area Specific Variables")]
    [SerializeField] private RectTransform blackjackBanner = null;
    [SerializeField] private TextMeshProUGUI winBannerText = null;
    [SerializeField] private RectTransform pushBanner = null;
    [SerializeField] private TextMeshProUGUI pushBannerText = null;
    [SerializeField] private WagerScriptableObject wager = null;
    [SerializeField] private TextMeshProUGUI availableCreditsText = null;
    [SerializeField] private TextMeshProUGUI currentBetText = null;
    [SerializeField] private GameObject doubledIcon = null;
    [SerializeField] private GameObject betTooltip = null;
    [SerializeField] private Button placeBetButton = null;
    [SerializeField] private Button betUpButton = null;
    [SerializeField] private Button betDownButton = null;
    [SerializeField] private Button standButton = null;
    [SerializeField] private Button splitButton = null;
    [SerializeField] private Button doubleDownButton = null;
    [SerializeField] private Button hitButton = null;

    protected override void Start()
    {
        base.Start();

        if (wager == null)
        {
            Debug.LogError("WagerScriptableObject not assigned, UI will not work properly");
            return;
        }

        //set up all the button onClick
        if (currentBetText != null) currentBetText.text = wager.GetCurrentBet.ToString();
        if (placeBetButton != null) placeBetButton.onClick.AddListener(OnBetButtonClicked);
        if (betUpButton != null) betUpButton.onClick.AddListener(OnBetUpButtonClicked);
        if (betDownButton != null) betDownButton.onClick.AddListener(OnBetDownButtonClicked);

        if (standButton != null) standButton.onClick.AddListener(OnStandButtonClicked);
        if (hitButton != null) hitButton.onClick.AddListener(OnHitButtonClicked);
        if (doubleDownButton != null) doubleDownButton.onClick.AddListener(OnDoubleButtonClicked);

        ToggleChangeBetButtons();

        if (doubledIcon != null) doubledIcon.SetActive(false);

        if (betTooltip != null) betTooltip.SetActive(true);
    }
    #region PRIVATE METHODS
    private void UpdateCreditText(float val) //used to animate the credit tick up animation on the text
    {
        int rounded = Mathf.RoundToInt(val);
        availableCreditsText.text = string.Format(CREDITS_TEXT_PREFIX, rounded);
    }

    private void OnBetButtonClicked()
    {
        if (!wager.CanPlaceBet)
        {
            BlackjackGameManager.Instance.ShowAddCoinPopup();
            return;
        }

        placeBetButton.interactable = false;
        betUpButton.interactable = false;
        betDownButton.interactable = false;

        if (betTooltip != null) betTooltip.SetActive(false);

        wager.PlaceBet();
        availableCreditsText.text = string.Format(CREDITS_TEXT_PREFIX, BlackjackGameManager.Instance.GetPlayerCredits);
        if (BlackjackGameManager.Instance != null) BlackjackGameManager.Instance.StartBlackjack();
    }

    private void ToggleChangeBetButtons()
    {
        //set the bet up and bet down button state based on if current can be changed or not
        betUpButton.interactable = !wager.IsCurrentBetMax && wager.CanIncreaseBet;
        betDownButton.interactable = !wager.IsCurrentBetMin;
    }

    private void OnBetUpButtonClicked()
    {
        wager.IncreaseBet();
        ToggleChangeBetButtons();
        currentBetText.text = wager.GetCurrentBet.ToString();
    }

    private void OnBetDownButtonClicked()
    {
        wager.DecreaseBet();
        ToggleChangeBetButtons();
        currentBetText.text = wager.GetCurrentBet.ToString();
    }

    private void OnStandButtonClicked()
    {
        ToggleButtons(false);
        if (BlackjackGameManager.Instance != null) BlackjackGameManager.Instance.PlayerStand();
    }

    private void OnHitButtonClicked()
    {
        ToggleButtons(false);
        if (BlackjackGameManager.Instance != null) BlackjackGameManager.Instance.PlayerHit();
    }

    private void OnDoubleButtonClicked()
    {
        if (!wager.CanPlaceBet)
        {
            BlackjackGameManager.Instance.ShowAddCoinPopup();
            return;
        }

        ToggleButtons(false);
        if (BlackjackGameManager.Instance != null) BlackjackGameManager.Instance.PlayerDouble();
        availableCreditsText.text = string.Format(CREDITS_TEXT_PREFIX, BlackjackGameManager.Instance.GetPlayerCredits);
        doubledIcon.SetActive(true);
    }
    //toggle these buttons after certain action happens, so player cant accidentaly spam them
    private void ToggleButtons(bool active)
    {
        standButton.interactable = active;
        hitButton.interactable = active;
        doubleDownButton.interactable = active;
    }
    #endregion

    #region PUBLIC METHODS
    public override void UpdateUI(HandResult handResult, int goal, bool doubledDown = false)
    {
        base.UpdateUI(handResult, goal); //update the shared UI reference

        //update the specific player area UI
        standButton.gameObject.SetActive(true);
        hitButton.gameObject.SetActive(true);
        doubleDownButton.gameObject.SetActive(true);

        if (doubledDown) return; //player automatically stand and can no longer hit after double down

        if(handResult.lowTotal < goal || handResult.highTotal < goal)
        {
            standButton.interactable = true;
            hitButton.interactable = true;
        }
    }

    //reset Player Area specific UI
    public override void SetDefaultState()
    {
        base.SetDefaultState();

        placeBetButton.interactable = true;
        ToggleChangeBetButtons();

        standButton.gameObject.SetActive(false);
        hitButton.gameObject.SetActive(false);
        doubleDownButton.gameObject.SetActive(false);

        ToggleButtons(true);

        blackjackBanner.gameObject.SetActive(false);
        pushBanner.gameObject.SetActive(false);

        doubledIcon.SetActive(false);

        currentBetText.text = wager.GetCurrentBet.ToString();
        RefreshPlayerCreditsDisplay();
    }

    public void RefreshPlayerCreditsDisplay()
    {
        if (availableCreditsText != null) availableCreditsText.text = string.Format(CREDITS_TEXT_PREFIX, BlackjackGameManager.Instance.GetPlayerCredits);
    }

    //show player's winning animation under different situations
    public override float ShowWinUI(BlackjackResult result)
    {
        float totalAnimationTime = 0;
        float winningTickupTime = 2f;

        //animate and show specific banner based on the win(payout) condition
        if (result == BlackjackResult.PlayerBlackjack) AnimateBanner(blackjackBanner);

        if (result == BlackjackResult.PlayerWin)
        {
            if (winBannerText != null) winBannerText.text = string.Format(WIN_BANNER_TEXT, wager.GetWinning);
            totalAnimationTime = AnimateBanner(winBanner);
        }

        if (result == BlackjackResult.Push)
        {
            if (pushBannerText != null) pushBannerText.text = string.Format(PUSH_BANNER_TEXT, wager.GetWinning);
            totalAnimationTime = AnimateBanner(pushBanner);
        }

        //show player's total credits going up
        DOVirtual.DelayedCall(totalAnimationTime, () =>
        {
            float prev = BlackjackGameManager.Instance.GetPlayerCredits - wager.GetWinning;
            float newVal = BlackjackGameManager.Instance.GetPlayerCredits;
            DOTween.To(UpdateCreditText, prev, newVal, winningTickupTime);
        });

        //this will return the total amount of time to complete all these animation
        //so the next step could wait that amount of time before they happen
        return totalAnimationTime + winningTickupTime; 
    }
    #endregion
}
