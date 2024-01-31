using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject chooseGameModeUI = null;
    [SerializeField] private GameObject pokerGameSelection = null;
    [SerializeField] private Button toPokerGameButton = null;
    [SerializeField] private Button toBlackjackButton = null;

    // Start is called before the first frame update
    void Start()
    {
        if (toPokerGameButton != null) toPokerGameButton.onClick.AddListener(OnToPokerButtonClicked);
        if (toBlackjackButton != null) toBlackjackButton.onClick.AddListener(OnToBlackjackButtonClicked);
    }

    public void OnToPokerButtonClicked()
    {
        if (chooseGameModeUI != null) chooseGameModeUI.SetActive(false);
        if (pokerGameSelection != null) pokerGameSelection.SetActive(true);
    }

    public void OnToBlackjackButtonClicked()
    {
        SceneManager.LoadScene("BlackjackGame");
    }
}

