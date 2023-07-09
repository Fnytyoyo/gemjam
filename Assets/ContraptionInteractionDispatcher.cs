using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ContraptionInteractionDispatcher : MonoBehaviour
{
    public OverlayLayer _OverlayLayer;
    public TileBase HoverTilePrefab;

    const char TRIM_CHAR = '_';

    public Color InteractionPossiblePrevewColor;

    Tilemap tilemap;
    Vector3Int? lastPos = null;

    private Vector3 cellHalfSize;

    public GameMode gameMode;

    public GameObject minePrefab;
    public GameObject jumpPadPrefab;
    public GameObject spikesPrefab;
    public GameObject cannonPrefab;

    public Dictionary<string, GameObject> tilePrefabsMap = new Dictionary<string, GameObject>();

    public Dictionary<Vector3Int, GameObject> tileObjectsMap;

    public Dictionary<Vector3Int, GameObject> playerTileObjectsMap = new Dictionary<Vector3Int, GameObject>();

    public void RechargePlayerContraptions()
    {
        CallOnRecharge(playerTileObjectsMap);
    }

    public void RechargeOnLevelContraptions()
    {
        CallOnRecharge(tileObjectsMap);
    }

    public static void CallOnRecharge(Dictionary<Vector3Int, GameObject> tileObjects)
    {
        UnityEngine.Debug.Log("[CallOnRecharge] STOSUNEK SEKUALNY " + tileObjects.Count);

        foreach (var c in tileObjects.Values)
        {
            var component = c.GetComponent<ContraptionBase>();
            if (component == null)
            {
                UnityEngine.Debug.Log("[CallOnRecharge] GameObject had not ContraptionBase component");
                continue;
            }

            component.OnRecharge();
        }
    }

    public void RechargeAllContraptions()
    {
        RechargePlayerContraptions();
        RechargeOnLevelContraptions();
    }

    void HandleRemoveBuilding(TileBase tile, Vector3Int cellPos)
    {
        if (playerTileObjectsMap.ContainsKey(cellPos))
        {
            var obj = playerTileObjectsMap[cellPos];
            GameObject.Destroy(obj);
            playerTileObjectsMap.Remove(cellPos);
            tilemap.SetTile(cellPos, null);
            gameMode.ChangeItemCount(TrimTileName(tile.name), +1);
        }
    }

    void HandleMouseDown()
    {
        var cellPos = tilemap.layoutGrid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var tile = tilemap.GetTile(cellPos);

        if (Input.GetMouseButtonDown(0))
        {
            switch (gameMode.currentAction)
            {
                case GameMode.ActionType.Interaction:
                    HandleTileInteraction(tile, cellPos);
                    break;
                case GameMode.ActionType.BuildSpikes:
                case GameMode.ActionType.BuildJumpPad:
                case GameMode.ActionType.BuildMine:
                case GameMode.ActionType.BuildCannon:
                    HandleBuilding(tile, cellPos, gameMode.currentAction);
                    break;
                default:
                    UnityEngine.Debug.Log("[ContraptionInteractionDispatcher::OnMouseDown()] Unknown ActionType");
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleRemoveBuilding(tile, cellPos);
        }
    }

    // Helper
    public static void RotateTile(Tilemap tilemap, Vector3Int coord, Quaternion rotation)
    {
        var oldMatrix = tilemap.GetTransformMatrix(coord);
        Matrix4x4 newMatrix = Matrix4x4.TRS(oldMatrix.GetPosition(), rotation, new Vector3(1, 1, 1));
        tilemap.SetTransformMatrix(coord, newMatrix);
    }

    private string TrimTileName(string tileName)
    {
        int floorPos = tileName.IndexOf(TRIM_CHAR);
        if (floorPos == -1)
        {
            return tileName;
        }
        return tileName.Substring(0, floorPos);
    }

    void Start()
    {
        gameMode = FindObjectOfType<GameMode>();

        tileObjectsMap = new Dictionary<Vector3Int, GameObject>();

        Camera.main.eventMask &= ~(1 << LayerMask.NameToLayer("Ragdoll"));

        tilePrefabsMap.Add("Mine", minePrefab);
        tilePrefabsMap.Add("JumpPad", jumpPadPrefab);
        tilePrefabsMap.Add("Spikes", spikesPrefab);
        tilePrefabsMap.Add("Cannon", cannonPrefab);

        tilemap = gameObject.GetComponent<Tilemap>();
        cellHalfSize = tilemap.cellSize / 2;
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

                        var newObj = Instantiate(prefabToSpawn, tilemap.CellToWorld(tileGridCoords) + cellHalfSize, tilemap.GetTransformMatrix(tileGridCoords).rotation);

                        tileObjectsMap.Add(tileGridCoords, newObj);
                    }
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GameMode>().IsInteractable == false)
        {
            _OverlayLayer.Clear();
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            HandleMouseDown();
        }

        HandleHovering();
    }

    public void HandleHovering()
    {
        var cellPos = tilemap.layoutGrid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var tile = tilemap.GetTile(cellPos);

        _OverlayLayer.Clear();

        if (gameMode.currentAction == GameMode.ActionType.Interaction)
        {
            if (tile != null)
            {
                if (tilePrefabsMap.ContainsKey(TrimTileName(tile.name)))
                {
                    _OverlayLayer.Set(cellPos, GetPreviewTileForAction(gameMode.currentAction));
                    _OverlayLayer.SetTileColor(cellPos, Color.green);
                }
            }
        }

        if (gameMode.IsInBuildingMode())
        {
            _OverlayLayer.Set(cellPos, GetPreviewTileForAction(gameMode.currentAction));
            _OverlayLayer.SetTileColor(cellPos, Color.black);
            _OverlayLayer.SetRotation(cellPos, Quaternion.Euler(0, 0, gameMode.buildingRotation * 90));

            if (gameMode.CanBuildOn(cellPos, gameMode.buildingRotation))
            {
                _OverlayLayer.SetTileColor(cellPos, Color.green);
            }
            else
            {
                _OverlayLayer.SetTileColor(cellPos, Color.red);
            }
        }

        lastPos = cellPos;
    }

    public TileBase GetPreviewTileForAction(GameMode.ActionType action)
    {
        if (FindObjectOfType<GameMode>().currentAction == GameMode.ActionType.Interaction)
        {
            return HoverTilePrefab;
        }

        if (gameMode.IsInBuildingMode())
        {
            var prefabObj = tilePrefabsMap[TrimTileName(ToTileString(action))];
            var component = prefabObj.GetComponent<ContraptionBase>();

            if (component != null)
            {
                return component.tile;
            }
        }

        return null;
    }

    private void HandleBuilding(TileBase tile, Vector3Int cellPos, GameMode.ActionType currentAction)
    {
        string contraptionName = ToTileString(gameMode.currentAction);
        var prefabToSpawn = tilePrefabsMap[TrimTileName(contraptionName)];

        if (prefabToSpawn == null)
        {
            UnityEngine.Debug.Log("[HandleBuilding] Wrong tileName");
            return;
        }

        if (FindObjectOfType<GameMode>().GetItemsLeft(contraptionName) == 0)
        {
            return;
        }

        var chosenRotation = gameMode.buildingRotation;
        if (gameMode.CanBuildOn(cellPos, chosenRotation) == false)
        {
            return;
        }

        var newObj = Instantiate(prefabToSpawn, tilemap.CellToWorld(cellPos) + cellHalfSize, Quaternion.Euler(0, 0, gameMode.buildingRotation * 90));
        playerTileObjectsMap.Add(cellPos, newObj);

        var component = newObj.GetComponent<ContraptionBase>();
        if (component == null)
        {
            UnityEngine.Debug.Log("[HandleBuilding] ContraptionBase component not found");
            return;
        }

        tilemap.SetTile(cellPos, component.tile);
        RotateTile(tilemap, cellPos, Quaternion.Euler(0, 0, gameMode.buildingRotation * 90));

        FindObjectOfType<GameMode>().ChangeItemCount(contraptionName, -1);
    }

    public static string ToTileString(GameMode.ActionType currentAction)
    {
        switch (currentAction)
        {
            case GameMode.ActionType.BuildSpikes:
                return "Spikes";
            case GameMode.ActionType.BuildJumpPad:
                return "JampPad";
            case GameMode.ActionType.BuildMine:
                return "Mine";
            case GameMode.ActionType.BuildCannon:
                return "Cannon";
            default:
                return "";
        }
    }

    public GameObject GetTileObject(Vector3Int gridCoords)
    {
        if (playerTileObjectsMap.ContainsKey(gridCoords))
        {
            return playerTileObjectsMap[gridCoords];
        }

        return tileObjectsMap[gridCoords];
    }

    private void HandleTileInteraction(TileBase tile, Vector3Int cellPos)
    {
        if (tile != null)
        {
            if (tilePrefabsMap.ContainsKey(TrimTileName(tile.name)))
            {
                const float EPS = 1f;
                bool isCloseEnough(float a, float b)
                {
                    return Mathf.Abs(a - b) < EPS;
                }

                int rotation = -1;
                float rotZ = tilemap.GetTransformMatrix(cellPos).rotation.eulerAngles.z;
                if (isCloseEnough(rotZ, 0))
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

                var tileObject = GetTileObject(cellPos);
                ContraptionBase component = null;
                tileObject.TryGetComponent<ContraptionBase>(out component);
                if (component != null)
                {
                    var worldPos = tilemap.CellToWorld(cellPos);
                    component.OnInteract(worldPos, rotation);
                }
            }
        }
    }
}
