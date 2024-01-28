using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PayoutContentUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Image backgroundImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateText(string text, bool isHighestPayout, bool rightAligned = false)
    {
        contentText.text = text;

        if (rightAligned) contentText.alignment = TextAlignmentOptions.Right;

        if (isHighestPayout) backgroundImage.color = Color.red;
    }
}
