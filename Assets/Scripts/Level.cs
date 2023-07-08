using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    [Serializable]
    public class ContraptionInventoryCount
    {
        public string ContraptionName;
        public Sprite ContraptionSprite;
        public int Count = 0;
        public string Shortcut;
    }

    [Serializable]
    public class StoryElement
    {
        [Multiline]
        public string StoryText = "";
        public float WaitBefore = 0.5f;
    }

    public Tilemap contraptionTilemap;
    public Tilemap wallsTilemap;

    public List<ContraptionInventoryCount> Inventory = new List<ContraptionInventoryCount>();

    public List<StoryElement> Story = new List<StoryElement> ();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetStartPosition()
    {
        GameObject startGO = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "Start")
            {
                startGO = transform.GetChild(i).gameObject;
                break;
            }
        }
        var startPos = startGO.transform.position;
        return startPos;
    }

    public Bounds GetBounds()
    {
        Vector3Int min = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        Vector3Int max = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

        Vector3 cellSize = Vector3.zero;

        foreach (var tm in GetComponentsInChildren<Tilemap>())
        {
            tm.CompressBounds();
            cellSize = tm.cellSize;
            min = Vector3Int.Min(min, tm.cellBounds.min);
            max = Vector3Int.Max(max, tm.cellBounds.max);
        }

        Bounds b = new Bounds();
        b.SetMinMax(
            new Vector3(
                min.x * cellSize.x,
                min.y * cellSize.y,
                min.z * cellSize.z
                ),
            new Vector3(
                max.x * cellSize.x,
                max.y * cellSize.y,
                max.z * cellSize.z
                )
            );
        return b;
    }
}
