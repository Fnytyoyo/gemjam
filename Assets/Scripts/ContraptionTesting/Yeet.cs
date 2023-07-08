using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yeet : MonoBehaviour
{
    public Rigidbody2D ToYeet;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("YEEET");
            ToYeet.AddForce(new Vector2(0, 5000));
        }
        
        if (Input.GetMouseButton(0))
        {
            var pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Debug.Log((pos - ToYeet.position).magnitude);
            ToYeet.AddForce(new Vector2(1, 1) * (1 / (pos - ToYeet.position).magnitude));
        }
    }
}
