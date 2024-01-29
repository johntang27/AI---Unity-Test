using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayoutTableUI : MonoBehaviour
{
    [SerializeField] private PayoutTableScriptableObject payoutTable = null;
    [SerializeField] private GridLayoutGroup payoutTableLayout = null;
    [SerializeField] private PayoutContentUI payoutContentUI = null;

    private List<PayoutContentUI>[] payoutTiers;
    private int cachedFlashingHandIndex = -1;
    private int cachedFlashingColIndex = -1;

    private void Start()
    {
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

            payoutTiers = new List<PayoutContentUI>[payoutTable.GetBetTierMultipliers.Count + 1];
            for(int i = 0; i < payoutTiers.Length; i++)
            {
                payoutTiers[i] = new List<PayoutContentUI>();
            }

            int col = 0;
            for (int i = 0; i < payoutTable.GetPayoutTableList.Count; i++)
            {
                PayoutContentUI payoutHand = Instantiate(payoutContentUI);
                payoutHand.UpdateText(payoutTable.GetPayoutTableList[i].GetPokerHandSO.GetDisplayName);
                payoutHand.transform.SetParent(payoutTableLayout.transform);
                payoutTiers[col].Add(payoutHand);

                for (int j = 0; j < payoutTable.GetBetTierMultipliers.Count; j++)
                {
                    col++;
                    PayoutContentUI payoutAmount = Instantiate(payoutContentUI);
                    payoutAmount.UpdateText((payoutTable.GetPayoutTableList[i].GetBasePayout * payoutTable.GetBetTierMultipliers[j]).ToString(), true);
                    payoutAmount.transform.SetParent(payoutTableLayout.transform);
                    payoutTiers[col].Add(payoutAmount);
                }
                col = 0;
            }
        }

        HighlightPayoutTier(true);
    }

    public void HighlightPayoutTier(bool on)
    {
        for(int i = 0; i < payoutTiers[PokerGameManager.Instance.GetCurrentBet].Count; i++)
        {
            payoutTiers[PokerGameManager.Instance.GetCurrentBet][i].HighlightBackground(on);
        }
    }
    
    public void FlashWinningPayout(int handIndex, int colIndex)
    {
        cachedFlashingHandIndex = handIndex;
        cachedFlashingColIndex = colIndex;

        payoutTiers[0][handIndex].ToggleTextFlash(true);
        payoutTiers[colIndex][handIndex].ToggleTextFlash(true);
    }

    public void DisableFlashingPayoutContent()
    {
        if (cachedFlashingHandIndex != -1 && cachedFlashingColIndex != -1)
        {
            payoutTiers[0][cachedFlashingHandIndex].ToggleTextFlash(false);
            payoutTiers[cachedFlashingColIndex][cachedFlashingHandIndex].ToggleTextFlash(false);

            cachedFlashingHandIndex = -1;
            cachedFlashingColIndex = -1;
        }
    }
}
