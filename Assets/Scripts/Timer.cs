using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI textVictory;
    public List<GameObject> UIs;
    private float currTime = 0f;
    
    void Start()
    {
        currTime = 0f;
    }

    void Update()
    {
        foreach (var ui in UIs)
        {
            if (ui.activeInHierarchy)
                return;
        }
        currTime += Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currTime);
        string timeStr = time.ToString(@"mm\:ss\.fff");
        text.text = timeStr;
        textVictory.text = timeStr;
    }
}
