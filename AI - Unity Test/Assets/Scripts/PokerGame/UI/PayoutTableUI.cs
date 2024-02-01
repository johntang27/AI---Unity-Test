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

    public void Init()
    {
        if (payoutTable != null)
        {
            if (payoutTableLayout != null)
            {
                //update the layout column constraint to account for the name of the hand and the number of bet multipliers
                payoutTableLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                payoutTableLayout.constraintCount = payoutTable.GetBetTierMultipliers.Count + 1;

                //evenly calculate the cell size based on the possible columns and rows
                float cellX = payoutTableLayout.GetComponent<RectTransform>().sizeDelta.x / payoutTableLayout.constraintCount;
                float cellY = payoutTableLayout.GetComponent<RectTransform>().sizeDelta.y / payoutTable.GetPayoutTableList.Count;

                payoutTableLayout.cellSize = new Vector2(cellX, cellY);
            }

            //create a list of payout grouped by the corresponding column they are in for that same bet multiplier
            payoutTiers = new List<PayoutContentUI>[payoutTable.GetBetTierMultipliers.Count + 1];
            for(int i = 0; i < payoutTiers.Length; i++) //index 0 is for containing the name of winning hand combos
            {
                payoutTiers[i] = new List<PayoutContentUI>();
            }

            int col = 0;
            for (int i = 0; i < payoutTable.GetPayoutTableList.Count; i++)
            {
                //create the hand combination name element
                PayoutContentUI payoutHand = Instantiate(payoutContentUI);
                payoutHand.UpdateText(payoutTable.GetPayoutTableList[i].GetPokerHandSO.GetDisplayName);
                payoutHand.transform.SetParent(payoutTableLayout.transform);
                payoutTiers[col].Add(payoutHand); //column of the names of winning hand combo

                for (int j = 0; j < payoutTable.GetBetTierMultipliers.Count; j++)
                {
                    col++;
                    //create each payout amount per multiplier
                    PayoutContentUI payoutAmount = Instantiate(payoutContentUI);
                    payoutAmount.UpdateText((payoutTable.GetPayoutTableList[i].GetBasePayout * payoutTable.GetBetTierMultipliers[j]).ToString(), true);
                    payoutAmount.transform.SetParent(payoutTableLayout.transform);
                    payoutTiers[col].Add(payoutAmount); //add this current payout to the corresponding column
                }
                col = 0;
            }
        }

        HighlightPayoutTier(true);
    }

    //highlight the current payout column based on how much bet was placed down
    public void HighlightPayoutTier(bool on)
    {
        for(int i = 0; i < payoutTiers[PokerGameManager.Instance.GetCurrentBet].Count; i++)
        {
            payoutTiers[PokerGameManager.Instance.GetCurrentBet][i].HighlightBackground(on);
        }
    }
    
    public void FlashWinningPayout(int handIndex, int colIndex)
    {
        //cache it so we can turn off the flashing later when we deal a new hand
        cachedFlashingHandIndex = handIndex;
        cachedFlashingColIndex = colIndex;

        payoutTiers[0][handIndex].ToggleTextFlash(true); //index 0 is always the name of the winning combo
        payoutTiers[colIndex][handIndex].ToggleTextFlash(true); //corresponds to which bet(multiplier) the player is currently on
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
