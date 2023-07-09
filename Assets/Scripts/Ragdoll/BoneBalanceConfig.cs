using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BoneBalanceConfig", order = 1)]
public class BoneBalanceConfig : ScriptableObject
{
    public float targetRotation = 0f;
    public float getUpTime = 2.5f;
    public float velocityThreshold = 2f;
    public float maxForce = 75f;
    public float getUpSpeed = 15f;
    public bool swiftGetUp = false;
    public float bodyHeight = 1f;
    public float bodyHeightBias = 0.15f;
    public float bodyMoveStrength = 5f;
    

}
