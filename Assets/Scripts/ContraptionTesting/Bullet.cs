using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public AudioSource audioSource;
    public SpriteRenderer spriteRenderer;
    public bool hit = false;
    
    public float force = 40.0f;
    public GameObject particlesPrefab;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        transform.rotation = Quaternion.FromToRotation(Vector2.right, GetComponent<Rigidbody2D>().velocity);

        if (hit && !audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (hit) return;
        Vector2 direction = GetComponent<Rigidbody2D>().velocity.normalized;
        Vector2 position = col.transform.position;
        if (col.gameObject.layer == LayerMask.NameToLayer("Ragdoll"))
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * force, position, ForceMode2D.Impulse);
            Instantiate(particlesPrefab, new Vector3(col.contacts[0].point.x, col.contacts[0].point.y, 0), Quaternion.identity);
        }

        hit = true;
        spriteRenderer.enabled = false;
        audioSource.Play();
    }
}
