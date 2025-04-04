using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RowUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;

    public void SetData(Sprite iconSprite, int amount, string label)
    {
        icon.sprite = iconSprite;
        amountText.SetText($"{label}: {amount}");
    }
}
