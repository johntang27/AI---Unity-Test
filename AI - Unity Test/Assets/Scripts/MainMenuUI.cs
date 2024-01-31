using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject chooseGameModeUI = null;
    [SerializeField] private GameObject pokerGameSelection = null;
    [SerializeField] private GameObject addCreditsPopup = null;
    [SerializeField] private Button toPokerGameButton = null;
    [SerializeField] private Button quitButton = null;
    [SerializeField] private Button addCreditsButton = null;
    [SerializeField] private Button backToGameModeButton = null;

    // Start is called before the first frame update
    void Start()
    {
        if (toPokerGameButton != null) toPokerGameButton.onClick.AddListener(OnToPokerButtonClicked);
        if (addCreditsButton != null) addCreditsButton.onClick.AddListener(OnAddCreditsBtnClicked);
        if (quitButton != null) quitButton.onClick.AddListener(OnQuitButtonClicked);
        if (backToGameModeButton != null) backToGameModeButton.onClick.AddListener(OnBackToGameModeBtnClicked);
    }

    private void OnToPokerButtonClicked()
    {
        if (chooseGameModeUI != null) chooseGameModeUI.SetActive(false);
        if (pokerGameSelection != null) pokerGameSelection.SetActive(true);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    private void OnAddCreditsBtnClicked()
    {
        if (addCreditsPopup != null) addCreditsPopup.SetActive(true);
    }

    private void OnBackToGameModeBtnClicked()
    {
        pokerGameSelection.SetActive(false);
        chooseGameModeUI.SetActive(true);
    }
}

