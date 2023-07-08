using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryElement : MonoBehaviour
{
    public TextMeshProUGUI CountText;
    public RawImage PreviewImage;


    private string name = "";
    private int maxCount = -1;
    public void Setup(string name, Sprite sprite, int maxCount)
    {
        this.name = name;
        this.maxCount = maxCount;

        PreviewImage.texture = sprite.texture;

        UpdateText(maxCount);
    }

    private void Update()
    {
        //TODO: Query GameMode for current count
        //UpdateText(currentCount);
    }

    public void UpdateText(int currentCount)
    {
        CountText.text = $"{currentCount}/{maxCount}";
    }
}
