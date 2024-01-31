using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Animation_RepeatSideMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        if (rectTransform) rectTransform.DOAnchorPosX(-70f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
}
