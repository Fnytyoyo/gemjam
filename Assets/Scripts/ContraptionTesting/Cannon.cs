using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cannon : ContraptionBase
{
    public GameObject bullet;

    public GameObject directionGO;
    private Vector2 position;
    
    void Start()
    {
        position = new Vector2(transform.position.x, transform.position.y);
    }

    public override void OnRecharge()
    {
        /* NOTE: Cannons dont need to be recharged */
    }

    public override void OnInteract(Vector3 pos, int rotation)
    {
        GameObject newBullet = Instantiate(bullet, position, Quaternion.identity);

        Vector2 direction = new Vector2();

        switch(rotation)
        {
            case 0:
                direction = new Vector2(0, 1);
                break;
            case 1:
                direction = new Vector2(-1, 0);
                break;
            case 2:
                direction = new Vector2(0, -1);
                break;
            case 3:
                direction = new Vector2(1, 0);
                break;
        }
        Debug.Log(direction);

        var rb = newBullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * 10;
    }
}