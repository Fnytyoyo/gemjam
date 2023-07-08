using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugForce : MonoBehaviour
{
    public float force = 100;
    public float angularForce = 100;
    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {   
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            rb.AddTorque(angularForce * Mathf.Deg2Rad * rb.inertia, ForceMode2D.Impulse);
        }
    }
}
