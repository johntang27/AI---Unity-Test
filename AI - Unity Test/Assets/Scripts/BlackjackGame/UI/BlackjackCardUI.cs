using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlackjackCardUI : MonoBehaviour
{
    [SerializeField] private Image cardImage = null;
    [SerializeField] private Sprite cardBack = null;
    [SerializeField] private PlayerCard cardData;

    private bool isCardFacedown = false;
    public bool IsCardFacedown => isCardFacedown;

    #region PUBLIC METHODS
    public void Init(PlayerCard card, Transform destination, Transform p, bool isFacedown, Action onMoveComplete = null)
    {
        //cache the card data and facedown state
        cardData = card;
        isCardFacedown = isFacedown;

        //dealing animation
        Sequence seq = DOTween.Sequence();
        seq.Append(this.transform.DOMove(destination.position, 1f));
        seq.AppendCallback(() =>
        {
            this.transform.SetParent(p); //set to the card container
            if (cardImage != null)
            {
                if (!isFacedown) cardImage.sprite = card.cardSprite; //update the card sprite
            }
            if (onMoveComplete != null) onMoveComplete(); //call any methods that happens after the animation ends
        });
    }

    public void FlipFacedownCard()
    {
        if (!isCardFacedown) return;

        cardImage.sprite = cardData.cardSprite;
        isCardFacedown = false;
    }
    #endregion
}
