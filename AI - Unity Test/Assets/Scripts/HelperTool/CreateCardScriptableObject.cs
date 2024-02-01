using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Procedurally create CardScriptableObject based on card spritesheet
/// </summary>
public class CreateCardScriptableObject : MonoBehaviour
{
    [SerializeField] private CardSpriteScriptableObject cardSpriteSourceSO;
    [SerializeField] private string saveLocation;

    public CardSpriteScriptableObject GetCardSpriteSourceSO => cardSpriteSourceSO;
    public string GetSaveLocation => saveLocation;
}
