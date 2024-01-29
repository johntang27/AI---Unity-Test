using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackjackDealAnimation : MonoBehaviour
{
    [SerializeField] private Vector2 startingPosition;
    [SerializeField] private RectTransform dealerArea;
    [SerializeField] private RectTransform playerArea;

    RectTransform rectTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
        startingPosition = rectTransform.anchoredPosition;
    }

    IEnumerator Dealing(Vector2 destination, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startingPosition, destination, time);
            time += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = dealerArea.anchoredPosition;
    }

    public IEnumerator DealToPlayer()
    {
        yield return StartCoroutine(Dealing(playerArea.anchoredPosition, 0.5f));
    }

    public IEnumerator DealToDealer()
    {
        yield return StartCoroutine(Dealing(dealerArea.anchoredPosition, 0.5f));
    }
}
