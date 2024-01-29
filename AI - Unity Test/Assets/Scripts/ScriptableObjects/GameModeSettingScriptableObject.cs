using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSettingScriptableObject : ScriptableObject
{
    [SerializeField] protected GameMode gameMode;

    public GameMode GetGameMode => gameMode;
}
