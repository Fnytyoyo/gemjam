using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmition : MonoBehaviour
{
    private Vector2 prevSpeed;
    private Rigidbody2D body;
    private AudioSource audioSource;
    private float time = 0.0f;

    public float treshold = 1f;

    public float cooldown = 0.25f;
    
    private Vector2 Speed => body.velocity;
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        var speedDiff = prevSpeed.magnitude - Speed.magnitude;
        prevSpeed = Speed;
        if (speedDiff > treshold)
        {
            if (time < cooldown) return;
            time = 0;
            audioSource.Play();
        }
    }

}
