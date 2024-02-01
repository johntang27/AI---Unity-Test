using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerGameSelectionUI : MonoBehaviour
{
    [SerializeField] private GameModeSettingScriptableObject[] pokerGameSettings;
    [SerializeField] private MenuPokerGameButton menuPokerGameButtonPrefab = null;
    [SerializeField] private Transform selectionContainer = null;

    // Start is called before the first frame update
    void Start()
    {
        //create all the poker game selection buttons and assigned them the corresponding poker game setting
        for (int i = 0; i < pokerGameSettings.Length; i++)
        {
            MenuPokerGameButton gameButton = Instantiate(menuPokerGameButtonPrefab, selectionContainer);
            gameButton.InitializeUI(pokerGameSettings[i]);
        }
    }
}
