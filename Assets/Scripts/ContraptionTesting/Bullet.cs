using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float force = 40.0f;
    
    void Update()
    {
        transform.rotation = Quaternion.FromToRotation(Vector2.right, GetComponent<Rigidbody2D>().velocity);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 direction = GetComponent<Rigidbody2D>().velocity.normalized;
        Vector2 position = col.transform.position;
        if (col.gameObject.layer == LayerMask.NameToLayer("Ragdoll"))
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * force, position, ForceMode2D.Impulse);
        }
        
        Destroy(this.gameObject);
    }
}
