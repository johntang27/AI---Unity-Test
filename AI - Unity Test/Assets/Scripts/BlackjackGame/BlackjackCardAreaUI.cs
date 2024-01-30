using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlackjackCardAreaUI : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI currentTotalText = null;
    [SerializeField] protected Transform cardContainer = null;
    [SerializeField] protected GameObject blackjackBanner = null;
    [SerializeField] protected GameObject bustBanner = null;

    public Transform GetCardContainer => cardContainer;
    
    private HandResult handResult;

    protected virtual void Start()
    {
        SetDefaultState();
    }

    public virtual void UpdateUI(HandResult handResult, int goal)
    {
        this.handResult = handResult;

        currentTotalText.transform.parent.gameObject.SetActive(true);

        if (handResult.hasAceCard) //display current total based on if hand contain Ace or not
        {
            //if the high total with Ace exceeds the goal(21), we display only the low total
            //else we show both the low total and high total
            currentTotalText.text = handResult.highTotal > goal ? handResult.lowTotal.ToString() : string.Format("{0} or {1}", handResult.lowTotal, handResult.highTotal);
        }
        else 
            currentTotalText.text = handResult.lowTotal.ToString();

        if (handResult.lowTotal == goal || handResult.highTotal == goal)
        {
            currentTotalText.text = goal.ToString();
            blackjackBanner.SetActive(true);
        }

        if(handResult.lowTotal > goal && handResult.highTotal > goal)
        {
            bustBanner.SetActive(true);
        }
    }

    public void FlipUpDealerCard()
    {
        if (cardContainer.childCount > 0) cardContainer.GetChild(0).GetComponent<BlackjackCardUI>().FlipFacedownCard();
    }

    public void SetDefaultState()
    {
        if (currentTotalText != null) currentTotalText.transform.parent.gameObject.SetActive(false);

        foreach(Transform card in cardContainer)
        {
            Destroy(card.gameObject);
        }

        blackjackBanner.SetActive(false);
        bustBanner.SetActive(false);
    }
}
