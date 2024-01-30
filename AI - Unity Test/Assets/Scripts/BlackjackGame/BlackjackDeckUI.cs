using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackjackDeckUI : MonoBehaviour
{
    [SerializeField] private BlackjackCardAreaUI dealerArea = null;
    [SerializeField] private BlackjackCardAreaUI playerArea = null;
    [SerializeField] private Transform[] playerSplitsArea;
    [SerializeField] private BlackjackCardUI cardUIPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DealCardToPlayer(PlayerCard card, Action onDestinationReached = null)
    {
        if(card == null)
        {
            Debug.LogError("PlayerCard is null, cannot deal card to player");
            return;
        }

        BlackjackCardUI cardUI = Instantiate(cardUIPrefab, this.transform.position, Quaternion.identity, this.transform);
        cardUI.Init(card, playerArea.transform, playerArea.GetCardContainer, false, onDestinationReached);
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
}
