using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BlackjackCardAreaUI : MonoBehaviour
{
    private const float bannerAnimationTime = 1f;

    [SerializeField] protected TextMeshProUGUI currentTotalText = null;
    [SerializeField] protected Transform cardContainer = null;
    [SerializeField] protected GameObject bustBanner = null;
    [SerializeField] protected RectTransform winBanner = null;

    public Transform GetCardContainer => cardContainer;    
    private HandResult handResult;

    protected virtual void Start()
    {
        SetDefaultState();
    }
    #region PUBLIC METHODS
    public virtual void UpdateUI(HandResult handResult, int goal, bool doubledDown = false)
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
        }

        if(handResult.lowTotal > goal && handResult.highTotal > goal) //show busted banner
        {
            bustBanner.SetActive(true);
        }
    }

    public virtual void SetDefaultState() //set up the default UI of the play area
    {
        if (currentTotalText != null) currentTotalText.transform.parent.gameObject.SetActive(false);

        foreach(Transform card in cardContainer)
        {
            Destroy(card.gameObject);
        }

        bustBanner.SetActive(false);
        winBanner.gameObject.SetActive(false);
    }

    public virtual float ShowWinUI(BlackjackResult result)
    {
        return AnimateBanner(winBanner);
    }
       
    public void FlipUpDealerCard()
    {
        if (cardContainer.childCount > 0) cardContainer.GetChild(0).GetComponent<BlackjackCardUI>().FlipFacedownCard();
    }
    #endregion

    #region PROTECTED METHODS
    protected float AnimateBanner(RectTransform rect)
    {
        if (rect != null)
        {
            rect.gameObject.SetActive(true);
            rect.localScale = Vector3.zero;
            rect.DOScale(Vector3.one, bannerAnimationTime).SetEase(Ease.OutBounce);
        }

        return bannerAnimationTime;
    }
    #endregion
}
