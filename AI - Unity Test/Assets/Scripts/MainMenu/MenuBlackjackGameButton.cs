using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBlackjackGameButton : MonoBehaviour
{
    [SerializeField] private GameModeSettingScriptableObject blackjackSetting = null;

    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null) button.onClick.AddListener(OnMenuGameButtonClicked);
    }

    public void OnMenuGameButtonClicked()
    {
        if (blackjackSetting == null)
        {
            Debug.LogError("Blackjack game setting not there, cannot proceed to play game");
            return;
        }

        if (MainGameController.Instance != null) MainGameController.Instance.settingsToLoad = blackjackSetting;
        SceneManager.LoadScene("BlackjackGame");
    }
}
