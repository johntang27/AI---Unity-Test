using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSpriteScriptableObject", menuName = "ScriptableObjects/CardSpriteScriptableObject")]
public class CardSpriteScriptableObject : ScriptableObject
{
    [SerializeField] private string spriteSourcePath;
    [SerializeField] private string spriteName;

    public string GetSpriteSourcePath => spriteSourcePath;
    public string GetSpriteName => spriteName;
}
