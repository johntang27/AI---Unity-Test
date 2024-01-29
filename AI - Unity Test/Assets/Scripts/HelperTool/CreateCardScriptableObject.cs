using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCardScriptableObject : MonoBehaviour
{
    [SerializeField] private CardSpriteScriptableObject cardSpriteSourceSO;
    [SerializeField] private string saveLocation;

    public CardSpriteScriptableObject GetCardSpriteSourceSO => cardSpriteSourceSO;
    public string GetSaveLocation => saveLocation;
}
