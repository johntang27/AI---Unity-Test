using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlackjackPlayerAreaUI : BlackjackCardAreaUI
{
    [SerializeField] private TextMeshProUGUI availableCreditsText = null;
    [SerializeField] private TextMeshProUGUI currentWagerText = null;
    [SerializeField] private Button wagerUpButton = null;
    [SerializeField] private Button wagerDownButton = null;
    [SerializeField] private Button standButton = null;
    [SerializeField] private Button splitButton = null;
    [SerializeField] private Button doubleDownButton = null;
    [SerializeField] private Button hitButton = null;

    protected override void Start()
    {
        base.Start();

        if (standButton != null) standButton.onClick.AddListener(OnStandButtonClicked);
        if (hitButton != null) hitButton.onClick.AddListener(OnHitButtonClicked);
    }

    public override void UpdateUI(HandResult handResult, int goal)
    {
        base.UpdateUI(handResult, goal);        
    }

    private void OnStandButtonClicked()
    {
        if (BlackjackGameManager.Instance != null) BlackjackGameManager.Instance.PlayerStand();
    }

    private void OnHitButtonClicked()
    {
        if (BlackjackGameManager.Instance != null) BlackjackGameManager.Instance.PlayerHit();
    }
}
