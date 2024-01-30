using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSettingScriptableObject : ScriptableObject
{
    [SerializeField] protected GameMode gameMode;
    [SerializeField] private bool isAvailable = false;
    [SerializeField] private int playerStartingHandSize;
    [SerializeField] private string gameInfoUrl;
    [SerializeField] private PlayerDataScriptableObject playerData = null;

    public GameMode GetGameMode => gameMode;
    public bool IsAvailable => isAvailable;
    public int GetPlayerStartingHandSize => playerStartingHandSize;
    public string GetGameInfoUrl => gameInfoUrl;
    public PlayerDataScriptableObject GetPlayerData => playerData;
}
