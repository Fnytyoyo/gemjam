using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OverlayLayer : MonoBehaviour
{
    Vector3Int? lastCell = null;

    public void Set(Vector3Int cell, TileBase tile)
    {
        if(lastCell != null)
        {
            Clear();
        }

        gameObject.GetComponent<Tilemap>().SetTile(cell, tile);

        lastCell = cell;
    }

    public void Clear()
    {
        if(lastCell == null )
        {
            return;
        }
        gameObject.GetComponent<Tilemap>().SetTile(lastCell.Value, null);
    }

    public void SetTileColor(Vector3Int cell, Color color)
    {
        gameObject.GetComponent<Tilemap>().SetTileFlags(cell, TileFlags.None);
        gameObject.GetComponent<Tilemap>().SetColor(cell, color);
    }

    public void SetRotation(Vector3Int cell, Quaternion rotation)
    {
        var tilemap = gameObject.GetComponent<Tilemap>();
        var oldMatrix = tilemap.GetTransformMatrix(cell);
        Matrix4x4 newMatrix = Matrix4x4.TRS(oldMatrix.GetPosition(), rotation, new Vector3(1, 1, 1));
        tilemap.SetTransformMatrix(cell, newMatrix);
    }
}
