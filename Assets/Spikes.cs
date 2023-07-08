using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : ContraptionBase
{
    public Vector2 direction = Vector2.up;
    
    private bool extracted;

    private void OnTriggerStay2D(Collider2D other)
    {
        var body = other.transform.GetComponent<Rigidbody2D>();
        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;
    }
    
    public override void OnInteract(Vector3 pos, int rotation)
    {
        if (extracted)
        {
            transform.position -= new Vector3(direction.x, direction.y, 0);
        }
        else
        {
            transform.position += new Vector3(direction.x, direction.y, 0);
        }
        extracted = !extracted;
    }
}
