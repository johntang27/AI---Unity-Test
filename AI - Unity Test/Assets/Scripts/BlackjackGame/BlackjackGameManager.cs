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

    [SerializeField] private int playerBestResult = 0;

    public void StartBlackjack()
    {
        if (gameSetting != null)
        {
            gameSetting.GetWager.ValidateBet();

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

    IEnumerator DealStartingHandFromDeck()
    {
        int dealt = 0;
        int cardIndex = 0;
        bool dealerFirstCard = true;
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
        playerBestResult = playerStartingResult.highTotal > gameSetting.GetBlackjackGoal ? playerStartingResult.lowTotal : playerStartingResult.highTotal;

        dealerAreaUI.UpdateUI(CalculateHandTotal(gameSetting.GetDealerHand.GetCurrentHand, true), gameSetting.GetBlackjackGoal);
        playerAreauUI.UpdateUI(playerStartingResult, gameSetting.GetBlackjackGoal);

        if (playerStartingResult.highTotal == gameSetting.GetBlackjackGoal)
        {
            dealerAreaUI.FlipUpDealerCard();
            HandResult dealerResult = CalculateHandTotal(gameSetting.GetDealerHand.GetCurrentHand); //dealer starting hand result after reveal
            dealerAreaUI.UpdateUI(dealerResult, gameSetting.GetBlackjackGoal);

            if (dealerResult.highTotal != gameSetting.GetBlackjackGoal) roundResult = BlackjackResult.PlayerBlackjack;
            else if (dealerResult.highTotal == gameSetting.GetBlackjackGoal) roundResult = BlackjackResult.Push;
            ResolveRoundResult();
        }
    }

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

        if (negateDealerFirstCard)
        {
            highValue = hand[1].blackjackValue;
            hasAce = hand[1].cardValue == CardValue.Ace;
            lowValue = hasAce ? highValue - 10 : highValue;
        }

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

    IEnumerator DealerDrawCards(HandResult result)
    {
        if (result.lowTotal == gameSetting.GetBlackjackGoal || result.highTotal == gameSetting.GetBlackjackGoal)
        {
            roundResult = BlackjackResult.PlayerLose;
            ResolveRoundResult();
            yield break;
        }

        HandResult nextResult = result;

        while(nextResult.lowTotal < 17)
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
        yield return new WaitForSeconds(1.5f);
        if (nextResult.lowTotal > gameSetting.GetBlackjackGoal) roundResult = BlackjackResult.PlayerWin;
        else if (nextResult.lowTotal == playerBestResult) roundResult = BlackjackResult.Push;
        else if (nextResult.lowTotal > playerBestResult) roundResult = BlackjackResult.PlayerLose;
        else if (nextResult.lowTotal < playerBestResult) roundResult = BlackjackResult.PlayerWin;

        ResolveRoundResult();
    }

    public void PlayerHit(bool isBetDouble = false)
    {
        AddCardToHand(gameSetting.GetPlayerHand);

        HandResult playerResult = CalculateHandTotal(gameSetting.GetPlayerHand.GetCurrentHand);
        if (playerResult.lowTotal > gameSetting.GetBlackjackGoal) roundResult = BlackjackResult.PlayerLose;

        playerBestResult = playerResult.highTotal > gameSetting.GetBlackjackGoal ? playerResult.lowTotal : playerResult.highTotal;

        deckUI.DealCardToPlayer(gameSetting.GetPlayerHand.GetLastCardInHand(), 
            ()=> {
                playerAreauUI.UpdateUI(playerResult, gameSetting.GetBlackjackGoal, isBetDouble);
                if (roundResult != BlackjackResult.NoResult) ResolveRoundResult();
            },
        isBetDouble);

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
        PlayerHit(true);
        StartCoroutine(DelayDealerCheck());
    }

    private void ResolveRoundResult()
    {
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

        StartCoroutine(PrepareNextRound(bannerAnimationDuration));
    }

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

    public void ShowAddCoinPopup()
    {
        if (addCoinPopup != null && uiCanvas != null)
        {
            AddCoinPopup popup = Instantiate(addCoinPopup, uiCanvas);
            popup.OnClose = () => playerAreauUI.SetDefaultState();
        }
    }
}
