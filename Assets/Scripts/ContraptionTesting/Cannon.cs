using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cannon : MonoBehaviour
{
    public GameObject bullet;

    public GameObject directionGO;
    private Vector2 position;
    
    void Start()
    {
        position = new Vector2(transform.position.x, transform.position.y);
    }
    
    void OnMouseDown()
    {
        GameObject newBullet = Instantiate(bullet, position, Quaternion.identity);

        Vector2 directionPosition = new Vector2(directionGO.transform.position.x, directionGO.transform.position.y);
        Vector2 direction = directionPosition - position;

        var rb = newBullet.GetComponent<Rigidbody2D>();
        Debug.Log(direction);
        rb.velocity = direction * 10;
    }
    
    void Update()
    {
    }
}