using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackjackDeckUI : MonoBehaviour
{
    [SerializeField] private BlackjackCardAreaUI dealerArea = null;
    [SerializeField] private BlackjackCardAreaUI playerArea = null;
    [SerializeField] private Transform[] playerSplitsArea;
    [SerializeField] private BlackjackCardUI cardUIPrefab = null;

    #region PUBLIC METHODS
    public void DealCardToPlayer(PlayerCard card, Action onDestinationReached = null, bool isDouble = false)
    {
        if(card == null)
        {
            Debug.LogError("PlayerCard is null, cannot deal card to player");
            return;
        }

        Quaternion cardRot = Quaternion.identity;
        if (isDouble) cardRot = Quaternion.Euler(0, 0, 270); //double down, card will be dealt sideway

        BlackjackCardUI cardUI = Instantiate(cardUIPrefab, this.transform.position, cardRot, this.transform);
        cardUI.Init(card, playerArea.transform, playerArea.GetCardContainer, false, onDestinationReached);
        if (isDouble) cardUI.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.4f); //change the pivot so it's easier to see the card underneath
    }

    public void DealCardToDealer(PlayerCard card, bool isFirstCard, Action onDestinationReached = null)
    {
        if (card == null)
        {
            Debug.LogError("PlayerCard is null, cannot deal card to dealer");
            return;
        }

        BlackjackCardUI cardUI = Instantiate(cardUIPrefab, this.transform.position, Quaternion.identity, this.transform);
        cardUI.Init(card, dealerArea.transform, dealerArea.GetCardContainer, isFirstCard, onDestinationReached);
    }
    #endregion
}
