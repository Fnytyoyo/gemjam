using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limbs : MonoBehaviour
{
    public static List<Rigidbody2D> limbRigidBodies;
    public static List<Collider2D> limbColliders;
    
    void Start()
    {
        limbRigidBodies = new List<Rigidbody2D>(GetComponentsInChildren<Rigidbody2D>());
        limbColliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
    }
}
