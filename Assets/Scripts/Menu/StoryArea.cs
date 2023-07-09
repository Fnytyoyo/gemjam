using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryArea : MonoBehaviour
{
    public TMPro.TextMeshProUGUI StoryTextTMP;

    public Color playerColor = new Color(0f, 0f, 1f, 1f);
    public Color gameSystemColor = new Color(1f, 0f, 0f, 1f);

    public void SetText(string text, bool isPlayer)
    {
        if (isPlayer == true)
        {
            StoryTextTMP.text = "PLAYER: ";
        }

        StoryTextTMP.text += text;

        if (isPlayer)
        { 
            StoryTextTMP.color = playerColor;
        }
        else
        {
            StoryTextTMP.color = gameSystemColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var tmp in GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            var rt = tmp.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.y, tmp.renderedHeight);
            tmp.ForceMeshUpdate();
        }
    }
}
