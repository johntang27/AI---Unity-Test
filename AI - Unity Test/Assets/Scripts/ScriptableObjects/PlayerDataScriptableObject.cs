using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataScriptableObject", menuName = "ScriptableObjects/PlayerDataScriptableObject")]
public class PlayerDataScriptableObject : ScriptableObject
{
    private const string PLAYER_CREDITS_PREF = "PlayerCreditsPrefs";
    [SerializeField] private int startingAmount;

    public int PlayerCredits
    {
        get => PlayerPrefs.GetInt(PLAYER_CREDITS_PREF, startingAmount);
        set => PlayerPrefs.SetInt(PLAYER_CREDITS_PREF, value);
    }
}
