using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameController : Singleton<MainGameController>
{
    public GameModeSettingScriptableObject settingsToLoad = null;

    public override void Awake()
    {
        base.Awake();
        if (m_instance != null) DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnPokerButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }
}
