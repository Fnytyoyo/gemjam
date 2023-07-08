using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryElement : MonoBehaviour
{
    public TextMeshProUGUI CountText;
    public TextMeshProUGUI TooltipText;
    public RawImage PreviewImage;

    public Color ActiveTintColor = Color.green;

    private string contraptionName = "";
    private string tooltip = "";
    private int maxCount = -1;
    public void Setup(string name, Sprite sprite, int maxCount, string tooltip)
    {
        this.tooltip = tooltip;
        this.contraptionName = name;
        this.maxCount = maxCount;

        PreviewImage.texture = sprite.texture;
        TooltipText.text = tooltip;

        UpdateText(maxCount);
    }

    private void Update()
    {
        if(FindObjectOfType<GameMode>().ShortcutStringToAction(tooltip) == FindObjectOfType<GameMode>().currentAction)
        {
            GetComponent<Image>().color = ActiveTintColor;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }

        UpdateText(FindObjectOfType<GameMode>().GetItemsLeft(contraptionName));
    }

    public void UpdateText(int currentCount)
    {
        if(maxCount == -1)
        {
            CountText.text = "";
            return;
        }
        CountText.text = $"{currentCount}/{maxCount}";
    }
}
