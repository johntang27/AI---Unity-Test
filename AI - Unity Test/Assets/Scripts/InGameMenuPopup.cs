using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenuPopup : MonoBehaviour
{
    [SerializeField] private Button menuButton = null;
    [SerializeField] private Button returnToGameButton = null;
    [SerializeField] private Button quitGameButton = null;

    private void Start()
    {
        if (menuButton != null) menuButton.onClick.AddListener(OnMenuButtonClicked);
        if (returnToGameButton != null) returnToGameButton.onClick.AddListener(OnReturnToGameButtonClicked);
        if (quitGameButton != null) quitGameButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnMenuButtonClicked()
    {
        SceneManager.LoadScene("Menu");
    }

    private void OnReturnToGameButtonClicked()
    {
        Destroy(this.gameObject);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
