using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaintDisplay : MonoBehaviour
{
    public Image IconImage;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI PriceText;

    public void SetPaint(Paint paint)
    {
        IconImage.sprite = paint.PaintIcon;
        NameText.text = paint.PaintName;
        PriceText.text = $"${paint.Price:F2}";
    }
}