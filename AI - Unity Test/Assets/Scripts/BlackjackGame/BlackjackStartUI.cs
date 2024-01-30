using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackjackStartUI : MonoBehaviour
{
    [SerializeField] private Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        if (startButton != null) startButton.onClick.AddListener(OnStartButtonClicked);
    }

    public void ShowStartUI()
    {
        this.gameObject.SetActive(true);
    }

    private void OnStartButtonClicked()
    {
        this.gameObject.SetActive(false);
        if (BlackjackGameManager.Instance != null) BlackjackGameManager.Instance.StartBlackjack();
    }
}
