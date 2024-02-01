using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HandResult
{
    public int highTotal;
    public int lowTotal;
    public bool hasAceCard;
    public List<PlayerCard> possibleSplitCards;

    public HandResult(int high, int low, bool hasAce, PlayerCard card1 = null, PlayerCard card2 = null)
    {
        highTotal = high;
        lowTotal = low;
        hasAceCard = hasAce;
        possibleSplitCards = new List<PlayerCard>();

        if (card1 != null && card2 != null)
        {            
            possibleSplitCards.Add(card1);
            possibleSplitCards.Add(card2);
        }        
    }
}

public class BlackjackGameManager : Singleton<BlackjackGameManager>
{
    [SerializeField] private BlackjackSettingScriptableObject gameSetting = null;
    [SerializeField] private BlackjackDeckUI deckUI = null;
    [SerializeField] private BlackjackCardAreaUI dealerAreaUI = null;
    [SerializeField] private BlackjackPlayerAreaUI playerAreauUI = null;
    [SerializeField] private Transform uiCanvas = null;
    [SerializeField] private AddCoinPopup addCoinPopup = null;

    private BlackjackResult roundResult = BlackjackResult.NoResult;
    public int GetPlayerCredits => gameSetting.GetPlayerData.PlayerCredits;
    public int GetCurrentBet => gameSetting.GetWager.GetCurrentBet;

    private int playerBestResult = 0;

    private void Start()
    {
        if (gameSetting == null)
        {
            if (MainGameController.Instance != null)
            {
                gameSetting = (BlackjackSettingScriptableObject)MainGameController.Instance.settingsToLoad;
            }            
        }
    }
    #region PUBLIC METHODS
    public void StartBlackjack()
    {
        if (gameSetting != null)
        {
            gameSetting.GetWager.ValidateBet(); //validate and update the bet after a double down attempt, in case the current bet exceed the bet range allowed

            roundResult = BlackjackResult.NoResult;

            gameSetting.GetPlayerHand.InitializeHand(gameSetting.GetPlayerStartingHandSize);
            gameSetting.GetDealerHand.InitializeHand(gameSetting.GetPlayerStartingHandSize);

            for (int i = 0; i < gameSetting.GetPlayerStartingHandSize; i++)
            {
                gameSetting.GetPlayerHand.DrawNextCardFromDeck();
                gameSetting.GetDealerHand.DrawNextCardFromDeck();
            }

            StartCoroutine(DealStartingHandFromDeck());
        }
    }

    public void ShowAddCoinPopup()
    {
        if (addCoinPopup != null && uiCanvas != null)
        {
            AddCoinPopup popup = Instantiate(addCoinPopup, uiCanvas);
            popup.OnClose = () => playerAreauUI.RefreshPlayerCreditsDisplay();
        }
    }

    public void PlayerHit(bool isBetDouble = false)
    {
        AddCardToHand(gameSetting.GetPlayerHand);

        HandResult playerResult = CalculateHandTotal(gameSetting.GetPlayerHand.GetCurrentHand);
        if (playerResult.lowTotal > gameSetting.GetBlackjackGoal) roundResult = BlackjackResult.PlayerLose; //player bust

        playerBestResult = playerResult.highTotal > gameSetting.GetBlackjackGoal ? playerResult.lowTotal : playerResult.highTotal;

        deckUI.DealCardToPlayer(gameSetting.GetPlayerHand.GetLastCardInHand(),
            () => {
                playerAreauUI.UpdateUI(playerResult, gameSetting.GetBlackjackGoal, isBetDouble);
                if (roundResult != BlackjackResult.NoResult) ResolveRoundResult();
            },
        isBetDouble);

        //when player hand reaches 21, we want to have the dealer take their turn
        if (playerResult.lowTotal == gameSetting.GetBlackjackGoal || playerResult.highTotal == gameSetting.GetBlackjackGoal) StartCoroutine(DelayDealerCheck());
    }

    public void PlayerStand()
    {
        dealerAreaUI.FlipUpDealerCard();
        HandResult dealerResult = CalculateHandTotal(gameSetting.GetDealerHand.GetCurrentHand); //dealer starting hand result after reveal
        dealerAreaUI.UpdateUI(dealerResult, gameSetting.GetBlackjackGoal);
        StartCoroutine(DealerDrawCards(dealerResult));
    }

    public void PlayerDouble()
    {
        gameSetting.GetWager.DoubleDownBet();
        PlayerHit(true); //player only take one card, then we go directly to dealer's turn
        StartCoroutine(DelayDealerCheck());
    }
    #endregion

    #region PRIVATE METHODS
    private HandResult CalculateHandTotal(List<PlayerCard> hand, bool negateDealerFirstCard = false)
    {
        int highValue = hand.Sum(card => card.blackjackValue); //Ace has a default value of 11
        int aceCount = hand.Where(card => card.cardValue == CardValue.Ace).Count();
        bool hasAce = aceCount > 0;

        //with multiple Aces in hand, we want to cout at least one Ace as 11, and the rest as 1
        if (aceCount > 1) highValue -= (aceCount - 1) * 10;

        //with Ace, it can either be counted as 1 or 11, so we cached the lowest total value using Ace as 1
        //we subtract 10 from highest total based on how many Aces are in the hand
        int lowValue = hasAce ? hand.Sum(card => card.blackjackValue) - 10 * aceCount : highValue;

        //dealer's first card is dealt facedown, so we do not want to count that value
        if (negateDealerFirstCard)
        {
            highValue = hand[1].blackjackValue;
            hasAce = hand[1].cardValue == CardValue.Ace;
            lowValue = hasAce ? highValue - 10 : highValue;
        }

        //FUTURE IMPLEMENTATION, cache the cards to handle a possible split
        if (hand.Count == 2)
        {
            if (hand[0].cardValue == hand[1].cardValue)
            {
                return new HandResult(highValue, lowValue, hasAce, hand[0], hand[1]);
            }
        }

        return new HandResult(highValue, lowValue, hasAce);
    }

    private void AddCardToHand(BlackjackHandScriptableObject hand)
    {
        hand.DrawNextCardFromDeck();
    }

    private void ResolveRoundResult()
    {
        //add up all the time it would take to complete the corresponding animations
        float bannerAnimationDuration = 0;

        if (roundResult == BlackjackResult.PlayerLose)
            bannerAnimationDuration = dealerAreaUI.ShowWinUI(roundResult);
        else if (roundResult == BlackjackResult.NoResult)
            Debug.LogError("THIS SHOULDN'T HAPPEN");
        else
        {
            gameSetting.GetWager.AwardWinning(roundResult);
            bannerAnimationDuration = playerAreauUI.ShowWinUI(roundResult);
        }

        //pass in the total and use it as a delay to reset all the play area UI
        StartCoroutine(PrepareNextRound(bannerAnimationDuration));
    }

    IEnumerator DealStartingHandFromDeck()
    {
        int dealt = 0;
        int cardIndex = 0;
        bool dealerFirstCard = true;
        //take turns dealing out the starting hand to player and dealer
        while (dealt < 4)
        {
            if (dealt == 0 || dealt == 2)
            {
                deckUI.DealCardToPlayer(gameSetting.GetPlayerHand.GetCurrentHand[cardIndex]);
                dealt++;
                yield return new WaitForSeconds(0.5f);
            }
            if (dealt == 1 || dealt == 3)
            {
                deckUI.DealCardToDealer(gameSetting.GetDealerHand.GetCurrentHand[cardIndex], dealerFirstCard);
                dealt++;
                cardIndex++;
                dealerFirstCard = false;
                yield return new WaitForSeconds(0.5f);
            }
        }

        HandResult playerStartingResult = CalculateHandTotal(gameSetting.GetPlayerHand.GetCurrentHand);
        playerBestResult = playerStartingResult.highTotal > gameSetting.GetBlackjackGoal ? playerStartingResult.lowTotal : playerStartingResult.highTotal; //cache the player's starting hand value

        //update the dealer and player play area UI
        dealerAreaUI.UpdateUI(CalculateHandTotal(gameSetting.GetDealerHand.GetCurrentHand, true), gameSetting.GetBlackjackGoal);
        playerAreauUI.UpdateUI(playerStartingResult, gameSetting.GetBlackjackGoal);

        if (playerStartingResult.highTotal == gameSetting.GetBlackjackGoal) //player achieves blackjack on starting hand
        {
            //reveal dealer's facedown card and check for blackjack
            dealerAreaUI.FlipUpDealerCard();
            HandResult dealerResult = CalculateHandTotal(gameSetting.GetDealerHand.GetCurrentHand); //dealer starting hand result after reveal
            dealerAreaUI.UpdateUI(dealerResult, gameSetting.GetBlackjackGoal);

            //update the round result based on dealer's total
            if (dealerResult.highTotal != gameSetting.GetBlackjackGoal) roundResult = BlackjackResult.PlayerBlackjack;
            else if (dealerResult.highTotal == gameSetting.GetBlackjackGoal) roundResult = BlackjackResult.Push;

            ResolveRoundResult();
        }
    }

    //after player stands, we have the dealer draw cards until their hand is complete
    IEnumerator DealerDrawCards(HandResult result)
    {
        //dealer achieves 21 on the first card they drew, break out coroutine and proceed to round ending sequence
        if (result.lowTotal == gameSetting.GetBlackjackGoal || result.highTotal == gameSetting.GetBlackjackGoal)
        {
            roundResult = BlackjackResult.PlayerLose;
            ResolveRoundResult();
            yield break;
        }

        HandResult nextResult = result;

        while(nextResult.lowTotal < 17) //we have the dealer continue to draw cards if their current total is less than 17
        {
            //Debug.LogError("dealerResult is: " + nextResult.lowTotal);
            yield return new WaitForSeconds(0.5f);
            AddCardToHand(gameSetting.GetDealerHand);
            nextResult = CalculateHandTotal(gameSetting.GetDealerHand.GetCurrentHand);
            deckUI.DealCardToDealer(gameSetting.GetDealerHand.GetLastCardInHand(), false, () =>
            {
                dealerAreaUI.UpdateUI(nextResult, gameSetting.GetBlackjackGoal);
            });
        }
        //dealer's hand total is now greater than 16, so we now handle the result and compare with player's total
        yield return new WaitForSeconds(1.5f);
        if (nextResult.lowTotal > gameSetting.GetBlackjackGoal) roundResult = BlackjackResult.PlayerWin;
        else if (nextResult.lowTotal == playerBestResult) roundResult = BlackjackResult.Push;
        else if (nextResult.lowTotal > playerBestResult) roundResult = BlackjackResult.PlayerLose;
        else if (nextResult.lowTotal < playerBestResult) roundResult = BlackjackResult.PlayerWin;

        ResolveRoundResult();
    }

    //reset the dealer and player area UI for the new round
    IEnumerator PrepareNextRound(float delay)
    {
        yield return new WaitForSeconds(delay + 1);
        gameSetting.GetWager.ValidateBet();
        dealerAreaUI.SetDefaultState();
        playerAreauUI.SetDefaultState();
    }

    IEnumerator DelayDealerCheck()
    {
        yield return new WaitForSeconds(1f);
        if (roundResult == BlackjackResult.NoResult) PlayerStand();
    }
    #endregion
}
