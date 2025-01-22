using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintMenuManager : MonoBehaviour
{
    [System.Serializable]
    public class PaintCategory
    {
        public string Name;
        public List<Paint> PaintItems;
    }

    [Header("Categories")]
    public List<PaintCategory> Categories;

    [Header("UI Elements")]
    public Transform PaintContent;
    public GameObject PaintPrefabDisplay;
    public Button[] SubmenuButtons;

    private void Start()
    {
        for (int i = 0; i < SubmenuButtons.Length; i++)
        {
            int index = i;
            SubmenuButtons[i].onClick.AddListener(() => ShowFurnitureMenu(index));
        }

        ShowFurnitureMenu(0);
    }
    public void ShowFurnitureMenu(int categoryIndex)
    {
        foreach (Transform child in PaintContent)
        {
            Destroy(child.gameObject);
        }

        if (categoryIndex < 0 || categoryIndex >= Categories.Count) return;
        var selectedCategory = Categories[categoryIndex];

        foreach (var paint in selectedCategory.PaintItems)
        {
            GameObject item = Instantiate(PaintPrefabDisplay, PaintContent);
            PaintDisplay display = item.GetComponent<PaintDisplay>();
            display.SetPaint(paint);

            Button itemButton = item.GetComponent<Button>();
            if (itemButton != null)
            {
                itemButton.onClick.AddListener(() => SelectPaint(paint));
            }
        }
    }

    private void SelectPaint(Paint paint)
    {
        Debug.Log($"Selected paint: {paint.PaintName} - ${paint.Price:F2}");
        // Implement logic to apply paint in the scene
    }
}
