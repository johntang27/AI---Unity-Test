using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the location of the SpriteSheet used for cards
/// Simply changing the location and sprite sheet name will allow you to easily update the card sprites
/// </summary>
[CreateAssetMenu(fileName = "CardSpriteScriptableObject", menuName = "ScriptableObjects/CardSpriteScriptableObject")]
public class CardSpriteScriptableObject : ScriptableObject
{
    [SerializeField] private string spriteSourcePath;
    [SerializeField] private string spriteName;

    public string GetSpriteSourcePath => spriteSourcePath;
    public string GetSpriteName => spriteName;
}
