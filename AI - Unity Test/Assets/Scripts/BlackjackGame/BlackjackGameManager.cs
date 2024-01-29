using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackjackGameManager : Singleton<BlackjackGameManager>
{
    [SerializeField] private BlackjackSettingScriptableObject gameSetting = null;
    [SerializeField] private BlackjackDealAnimation dealAnimation = null;

    // Start is called before the first frame update
    void Start()
    {
        if (gameSetting != null)
        {
            gameSetting.GetPlayerHand.InitializeHand(gameSetting.GetPlayerStartingHandSize);
            gameSetting.GetDealerHand.InitializeHand(gameSetting.GetPlayerStartingHandSize);

            for(int i = 0; i < gameSetting.GetPlayerStartingHandSize * 2; i+=2)
            {
                gameSetting.GetPlayerHand.DrawCardFromDeck(i);
                gameSetting.GetDealerHand.DrawCardFromDeck(i + 1);
            }

            StartCoroutine(PlayDealingAnimation());
        }
    }

    IEnumerator PlayDealingAnimation()
    {
        yield return StartCoroutine(dealAnimation.DealToPlayer());
        yield return StartCoroutine(dealAnimation.DealToDealer());
        yield return StartCoroutine(dealAnimation.DealToPlayer());
        yield return StartCoroutine(dealAnimation.DealToDealer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
