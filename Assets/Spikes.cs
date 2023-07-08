using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : ContraptionBase
{
    private Vector2 direction = Vector2.up;
    private bool extracted;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!extracted) return;
        var body = other.transform.GetComponent<Rigidbody2D>();
        body.velocity = Vector2.zero;
        body.angularVelocity = 0f;
    }
    
    public override void OnInteract(Vector3 pos, int rotation)
    {
        direction = rotation switch
        {
            0 => Vector2.up,
            1 => Vector2.right,
            2 => Vector2.down,
            3 => Vector2.left,
            _ => direction
        };
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
