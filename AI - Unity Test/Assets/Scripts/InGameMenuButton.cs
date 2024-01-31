using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuButton : MonoBehaviour
{
    [SerializeField] private Transform uiCanvas = null;
    [SerializeField] private InGameMenuPopup menuPopupPrefab = null;

    private void Start()
    {
        Button button = this.GetComponent<Button>();
        if (button != null) button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (uiCanvas != null && menuPopupPrefab != null) Instantiate(menuPopupPrefab, uiCanvas);
    }
}
