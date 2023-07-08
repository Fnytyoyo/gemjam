using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContraptionBase : MonoBehaviour
{
    public string ContraptionName;

    public abstract void OnInteract(Vector3 pos, int rotation);
}
