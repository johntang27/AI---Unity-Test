using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Blackjack's own ScriptableObject to handle special case of blackjack betting style
/// Awards the player corresponding winning based on the win result
/// </summary>
[CreateAssetMenu(fileName = "BlackjackWagerScriptableObject", menuName = "ScriptableObjects/WagerScriptableObject/BlackjackWagerScriptableObject")]
public class BlackjackWagerScriptableObject : WagerScriptableObject
{
    public override void DoubleDownBet()
    {
        PlaceBet();
        currentBet *= 2;
    }

    public override void AwardWinning(BlackjackResult result)
    {
        switch (result)
        {
            case BlackjackResult.Push:
                winning = currentBet;
                Debug.LogError("PUSH");
                break;
            case BlackjackResult.PlayerBlackjack:
                winning = Mathf.RoundToInt(currentBet * 2.5f);
                Debug.LogError("BLACKJACK!");
                break;
            case BlackjackResult.PlayerWin:
                winning = currentBet * 2;
                Debug.LogError("PLAYER WIN");
                break;
        }

        playerData.PlayerCredits += winning;
    }
}
