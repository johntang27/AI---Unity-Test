using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSettingScriptableObject : ScriptableObject
{
    [SerializeField] protected GameMode gameMode;
    [SerializeField] private bool isAvailable = false;

    public GameMode GetGameMode => gameMode;
    public bool IsAvailable => isAvailable;
}
