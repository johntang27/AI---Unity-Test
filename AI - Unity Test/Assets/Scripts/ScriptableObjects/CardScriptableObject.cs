using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScriptableObject", menuName = "ScriptableObjects/CardScriptableObject")]
public class CardScriptableObject : ScriptableObject
{
    [SerializeField] private string cardName;
    [SerializeField] private CardSuit cardSuit;
    [SerializeField] private CardValue cardValue;
    [SerializeField] private Sprite cardSprite;

    public void UpdateData(Sprite sprite)
    {
        string[] splits = this.name.Split('_');

        cardName = this.name;

        if (splits.Length == 1) return; //joker card

        bool canGetValue = System.Enum.TryParse(splits[0], out cardValue);
        if (!canGetValue) Debug.LogErrorFormat("cannot parse {0} card value from name", splits[0]);

        bool canGetSuit = System.Enum.TryParse(splits[1], out cardSuit);
        if (!canGetSuit) Debug.LogErrorFormat("cannot parse {0} card suit from name", splits[1]);

        if (sprite != null) cardSprite = sprite;
        else Debug.LogErrorFormat("cannot find sprite for {0}", cardName);
    }
}
