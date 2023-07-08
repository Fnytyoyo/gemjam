using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ContraptionInteractionDispatcher : MonoBehaviour
{
    public OverlayLayer _OverlayLayer;
    public TileBase HoverTilePrefab;

    const char TRIM_CHAR = '_';

    public Color InteractionPossiblePrevewColor;

    private Dictionary<string, ContraptionBase> contraptionsMap = new Dictionary<string, ContraptionBase>();
    Tilemap tilemap;
    Vector3Int? lastPos = null;

    public GameObject minePrefab;
    public GameObject jumpPadPrefab;
    public GameObject spikesPrefab;
    public GameObject cannonPrefab;

    public Dictionary<string, GameObject> tilePrefabsMap = new Dictionary<string, GameObject>();
    
    public Dictionary<Vector3Int, GameObject> tileObjectsMap = new Dictionary<Vector3Int, GameObject>();

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
        tilePrefabsMap.Add("Mine", minePrefab);
        tilePrefabsMap.Add("JumpPad", jumpPadPrefab);
        tilePrefabsMap.Add("Spikes", spikesPrefab);
        tilePrefabsMap.Add("Cannon", cannonPrefab);

        foreach ( var c in gameObject.GetComponents<ContraptionBase>())
        {
            contraptionsMap.Add(c.ContraptionName, c);
        }

        tilemap = gameObject.GetComponent<Tilemap>();
        lastPos = null;

        for (int i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; ++i)
        {
            for (int j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; ++j)
            {
                var tileGridCoords = new Vector3Int(i, j, 0);
                var tile = tilemap.GetTile(tileGridCoords);
                if (tile != null)
                {
                    if (tilePrefabsMap.ContainsKey(TrimTileName(tile.name)))
                    {
                        var prefabToSpawn = tilePrefabsMap[TrimTileName(tile.name)];
                        var newObj = Instantiate(prefabToSpawn, tilemap.CellToWorld(tileGridCoords), Quaternion.identity);

                        tileObjectsMap.Add(tileGridCoords, newObj);
                    }
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var cellPos = tilemap.layoutGrid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        var tile = tilemap.GetTile(cellPos);

        _OverlayLayer.Clear();
        if (tile != null)
        {
            if(contraptionsMap.ContainsKey(TrimTileName(tile.name)))
            {
                _OverlayLayer.Set(cellPos, HoverTilePrefab);
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
