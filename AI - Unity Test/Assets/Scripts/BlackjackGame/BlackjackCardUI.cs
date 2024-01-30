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

    public void Init(PlayerCard card, Transform destination, Transform p, bool isFacedown, Action onMoveComplete = null)
    {
        cardData = card;
        isCardFacedown = isFacedown;

        Sequence seq = DOTween.Sequence();
        seq.Append(this.transform.DOMove(destination.position, 1f));
        seq.AppendCallback(() =>
        {
            this.transform.SetParent(p);
            if (cardImage != null)
            {
                if (!isFacedown) cardImage.sprite = card.cardSprite;
            }
            if (onMoveComplete != null) onMoveComplete();
        });
    }

    public void FlipFacedownCard()
    {
        if (!isCardFacedown) return;

        cardImage.sprite = cardData.cardSprite;
        isCardFacedown = false;
    }
}
