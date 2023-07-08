using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ContraptionInteractionDispatcher : MonoBehaviour
{
    const char TRIM_CHAR = '_';

    public Color InteractionPossiblePrevewColor;

    private Dictionary<string, ContraptionBase> contraptionsMap = new Dictionary<string, ContraptionBase>();
    Tilemap tilemap;
    Vector3Int? lastPos = null;

    private string TrimTileName(string tileName)
    {
        int floorPos = tileName.IndexOf(TRIM_CHAR);
        if(floorPos == -1)
        {
            return tileName;
        }
        return tileName.Substring(0, floorPos);
    }

    void Start()
    {
        foreach( var c in gameObject.GetComponents<ContraptionBase>())
        {
            contraptionsMap.Add(c.ContraptionName, c);
        }

        tilemap = gameObject.GetComponent<Tilemap>();
        lastPos = null;
    }

    // Update is called once per frame
    void Update()
    {
        var cellPos = tilemap.layoutGrid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if(lastPos != null && lastPos == cellPos)
        {
            return;
        }

        if(lastPos != null)
        {
            tilemap.SetColor(lastPos.Value, Color.white);
        }

        var tile = tilemap.GetTile(cellPos);

        if (tile != null)
        {
            if(contraptionsMap.ContainsKey(TrimTileName(tile.name)))
            {
                tilemap.SetColor(cellPos, InteractionPossiblePrevewColor);
            }
        }

        lastPos = cellPos;
    }

    private void OnMouseDown()
    {
        var cellPos = tilemap.layoutGrid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var tile = tilemap.GetTile(cellPos);

        if (tile != null)
        {
            if (contraptionsMap.ContainsKey(TrimTileName(tile.name)))
            {
                const float EPS = 1f;
                bool isCloseEnough(float a, float b)
                {
                    return Mathf.Abs(a - b) < EPS;
                }


                int rotation = -1;
                float rotZ = tilemap.GetTransformMatrix(cellPos).rotation.eulerAngles.z;
                if ( isCloseEnough(rotZ, 0))
                {
                    rotation = 0;
                }
                else if (isCloseEnough(rotZ, 90))
                {
                    rotation = 1;
                }
                else if (isCloseEnough(rotZ, 180))
                {
                    rotation = 2;
                }
                else if (isCloseEnough(rotZ, 270))
                {
                    rotation = 3;
                }

                var worldPos = tilemap.CellToWorld(cellPos);
                contraptionsMap[TrimTileName(tile.name)].OnInteract(worldPos, rotation);
            }
        }
    }
}
