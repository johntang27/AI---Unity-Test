using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokerGameManager : MonoBehaviour
{
    [SerializeField] private PlayerHandScriptableObject playerHand = null;
    [SerializeField] private PayoutTableScriptableObject payoutTable = null;

    [SerializeField] private List<CardButtonUI> playerHandUI = new List<CardButtonUI>();
    [SerializeField] private Button messageBanner = null;
    [SerializeField] private Button dealButton = null;
    [SerializeField] private TextMeshProUGUI dealButtonText = null;
    [SerializeField] private GridLayoutGroup payoutTableLayout = null;
    [SerializeField] private PayoutContentUI payoutContentUI = null;
    [SerializeField] private TextMeshProUGUI currentHandText = null;

    private PokerGameState currentGameState = PokerGameState.Deal;
    // Start is called before the first frame update
    void Start()
    {
        currentGameState = PokerGameState.Deal;

        if (dealButton != null) dealButton.onClick.AddListener(OnDrawButtonClicked);

        if (payoutTable != null)
        {
            if (payoutTableLayout != null)
            {
                payoutTableLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                payoutTableLayout.constraintCount = payoutTable.GetBetTierMultipliers.Count + 1;

                float cellX = payoutTableLayout.GetComponent<RectTransform>().sizeDelta.x / payoutTableLayout.constraintCount;
                float cellY = payoutTableLayout.GetComponent<RectTransform>().sizeDelta.y / payoutTable.GetPayoutTableList.Count;

                payoutTableLayout.cellSize = new Vector2(cellX, cellY);
            }

            for (int i = 0; i < payoutTable.GetPayoutTableList.Count; i++)
            {
                PayoutContentUI payoutHand = Instantiate(payoutContentUI);
                payoutHand.UpdateText(payoutTable.GetPayoutTableList[i].GetPokerHandSO.GetDisplayName, false);
                payoutHand.transform.SetParent(payoutTableLayout.transform);

                for (int j = 0; j < payoutTable.GetBetTierMultipliers.Count; j++)
                {
                    PayoutContentUI payoutAmount = Instantiate(payoutContentUI);
                    payoutAmount.UpdateText((payoutTable.GetPayoutTableList[i].GetBasePayout * payoutTable.GetBetTierMultipliers[j]).ToString(), j == payoutTable.GetBetTierMultipliers.Count - 1, true);
                    payoutAmount.transform.SetParent(payoutTableLayout.transform);
                }
            }
        }
    }

    public void OnDrawButtonClicked()
    {
        if(currentGameState == PokerGameState.Deal)
        {
            if(playerHand == null)
            {
                Debug.LogError("Player Hand ScriptableObject not set, cannot start game");
                return;
            }

            playerHand.InitializeHand();

            //if (handEvaluator[0].IsHandValid()) Debug.LogError("Is Valid Hand");
            //else Debug.LogError("Not a Valid Hand");
            //return;

            if (playerHand.GetCurrentHand.Count != playerHandUI.Count)
            {
                Debug.LogError("Player Card Count does not match Card UI Count, cannot start game");
                return;
            }

            if (messageBanner != null) messageBanner.gameObject.SetActive(false);

            for (int i = 0; i < playerHand.GetCurrentHand.Count; i++)
            {
                playerHandUI[i].UpdateUI(playerHand.GetCurrentHand[i].cardSprite);
            }

            if (dealButtonText != null) dealButtonText.text = "DRAW";

            currentGameState = PokerGameState.ToDraw;

            for(int i = 0; i < payoutTable.GetPayoutTableList.Count; i++)
            {
                if (payoutTable.GetPayoutTableList[i].GetPokerHandSO.IsHandValid())
                {
                    if (currentHandText != null) currentHandText.text = payoutTable.GetPayoutTableList[i].GetPokerHandSO.GetDisplayName;
                    return;
                }
            }

            return;
        }

        if(currentGameState == PokerGameState.ToDraw)
        {
            for(int i = 0; i < playerHandUI.Count; i++)
            {
                if (playerHandUI[i].GetHeldState)
                {
                    playerHandUI[i].OnCardClicked();
                    continue;
                }

                playerHand.ReplaceCardFromDeck(i);
                playerHandUI[i].UpdateUI(playerHand.GetCurrentHand[i].cardSprite);
            }

            for (int i = 0; i < payoutTable.GetPayoutTableList.Count; i++)
            {
                if (payoutTable.GetPayoutTableList[i].GetPokerHandSO.IsHandValid())
                {
                    if (currentHandText != null) currentHandText.text = payoutTable.GetPayoutTableList[i].GetPokerHandSO.GetDisplayName;
                    return;
                }
            }

            if (dealButtonText != null) dealButtonText.text = "DEAL";

            currentGameState = PokerGameState.Deal;
        }
    }
}
