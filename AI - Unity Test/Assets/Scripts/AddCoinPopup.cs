using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddCoinPopup : MonoBehaviour
{
    [SerializeField] private PlayerDataScriptableObject playerData = null;
    [Multiline]
    [SerializeField] private string message;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI messageText = null;
    [SerializeField] private TextMeshProUGUI creditText = null;
    [SerializeField] private Button[] addCreditButtons;
    [SerializeField] private Button closeButton;

    public Action OnClose = null;

    // Start is called before the first frame update
    void Start()
    {
        if (messageText != null) messageText.text = message;
        if (creditText != null) creditText.text = playerData.PlayerCredits.ToString();

        for (int i = 0; i < addCreditButtons.Length; i++)
        {
            float amt = Mathf.Pow(10, i);
            addCreditButtons[i].onClick.AddListener(() =>
            {
                OnAddCreditButtonClicked(amt);
            });
        }

        if (closeButton != null) closeButton.onClick.AddListener(ClosePopup);
    }

    private void OnAddCreditButtonClicked(float amount)
    {
        playerData.PlayerCredits += (int)amount;
        creditText.text = playerData.PlayerCredits.ToString();
    }

    private void ClosePopup()
    {
        if (OnClose != null) OnClose();

        Destroy(this.gameObject);
    }
}
