using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PayoutContentUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color defaultBGColor;

    bool canTextFlash = false;

    public void UpdateText(string text, bool rightAligned = false)
    {
        if (contentText != null) contentText.text = text;

        if (rightAligned) contentText.alignment = TextAlignmentOptions.Right;
    }

    public void HighlightBackground(bool on)
    {
        if (on) backgroundImage.color = Color.red;
        else backgroundImage.color = defaultBGColor;
    }

    public void ToggleTextFlash(bool on)
    {
        canTextFlash = on;
        StartCoroutine(TextFlash());
    }

    IEnumerator TextFlash()
    {
        while (canTextFlash)
        {
            contentText.color = Color.white;
            yield return new WaitForSeconds(0.5f);
            contentText.color = Color.yellow;
            yield return new WaitForSeconds(0.5f);
        }
        contentText.color = Color.yellow;
    }
}
