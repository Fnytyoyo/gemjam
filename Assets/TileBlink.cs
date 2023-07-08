using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBlink : MonoBehaviour
{
    public Tilemap Map;
    public TileBase Tile;

    public float Period = 2.0f;

    float timer = 0.0f;
    bool visible = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;

        foreach (var pp in Map.cellBounds.allPositionsWithin)
        {
            var tile = Map.GetTile(pp);
            if(tile)
            {
                Debug.Log($"{pp}: {tile.name}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var pos = Map.layoutGrid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        timer += Time.deltaTime;

        if(timer >= Period)
        {
            timer -= Period;

            Map.SetTile(pos, visible ? null : Tile);
            visible = !visible;
        }
    }
}
