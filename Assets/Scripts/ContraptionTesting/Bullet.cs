using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.FromToRotation(Vector2.right, GetComponent<Rigidbody2D>().velocity);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ragdoll"))
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForce((col.gameObject.transform.position - transform.position) * 5000);
        }
        
        Destroy(this.gameObject);
    }
}
