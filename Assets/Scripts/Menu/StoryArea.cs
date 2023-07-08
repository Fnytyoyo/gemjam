using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryArea : MonoBehaviour
{
    public TMPro.TextMeshProUGUI StoryTextTMP;

    public void SetText(string text)
    {
        StoryTextTMP.text = text;
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
