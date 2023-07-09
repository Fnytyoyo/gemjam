using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class ContraptionBase : MonoBehaviour
{
    public string ContraptionName;

    public TileBase tile;

    public abstract void OnInteract(Vector3 pos, int rotation);

    public abstract void OnRecharge();
}
