using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButtonUI : MonoBehaviour
{
    [SerializeField] private Image cardImage = null;
    [SerializeField] private GameObject heldIndicator = null;

    private Button button = null;
    private bool isHeld = false;

    public bool GetHeldState => isHeld;

    private void Start()
    {
        button = this.GetComponent<Button>();
        if (button != null) button.onClick.AddListener(OnCardClicked);

        if (heldIndicator != null) heldIndicator.SetActive(false);
    }

    public void UpdateUI(Sprite cardSprite)
    {
        if (cardImage != null) cardImage.sprite = cardSprite;
    }

    public void OnCardClicked()
    {
        isHeld = !isHeld;
        heldIndicator.SetActive(isHeld);
    }

    public void ToggleInteraction(bool on)
    {
        button.interactable = on;
    }
}
