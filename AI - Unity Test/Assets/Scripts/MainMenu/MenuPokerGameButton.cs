using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuPokerGameButton : MonoBehaviour
{
    [SerializeField] private GameModeSettingScriptableObject gameSetting = null;
    [SerializeField] private TextMeshProUGUI gameNameText = null;
    [SerializeField] private GameObject comingSoonBlocker = null;

    private Button button = null;

    public void InitializeUI(GameModeSettingScriptableObject setting)
    {
        gameSetting = setting;

        button = this.GetComponent<Button>();
        if (button != null) button.onClick.AddListener(OnButtonClicked);

        if (gameNameText != null) gameNameText.text = ((PokerGameSettingScriptableObject)gameSetting).GetPokerSubGame.ToString().Replace('_', ' ');

        if (!gameSetting.IsAvailable)
        {
            comingSoonBlocker.SetActive(true);
            button.interactable = false;
        }
    }

    private void OnButtonClicked()
    {
        if (MainGameController.Instance != null) MainGameController.Instance.settingsToLoad = gameSetting;
        SceneManager.LoadScene("Game");
    }
}
