using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineContraption : ContraptionBase
{
    public override void OnInteract(Vector3 pos, int rotation)
    {
        Debug.Log($"BOOM! at {pos}, {rotation}");
    }
}
