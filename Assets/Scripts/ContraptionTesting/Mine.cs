using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mine : ContraptionBase
{
    public float explosionTime = 0.2f;
    public float basePower = 30.0f;

    public Color readyColor;
    public Color usedColor;

    public ParticleSystem particles;
    
    private Vector2 position;

    private bool readyToUse;
    
    void Start()
    {
        position = new Vector2(this.transform.position.x, this.transform.position.y);
        readyToUse = true;
    }

    private void ApplyForce(Rigidbody2D limb, float time)
    {
        var distance = (limb.position - this.position).magnitude;
        var forceDirection = (limb.position - this.position).normalized;

        var distanceModifier = 1 / distance;
        var timeModifier = 1 - Mathf.Pow((time / explosionTime), 2);
        
        limb.AddForce(forceDirection * basePower * distanceModifier * timeModifier);
    }
    
    private void ApplyForces(float time)
    {
        foreach (var limb in Limbs.limbRigidBodies)
        {
            ApplyForce(limb, time);
        }
    }
    
    IEnumerator Explode()
    {
        var time = 0.0f;
        while (time < explosionTime)
        {
            ApplyForces(time);
            time += Time.deltaTime;
            yield return null;
        }
    }
    
    public override void OnInteract(Vector3 pos, int rot)
    {
        if (readyToUse)
        {
            Debug.Log("Mine explosion");
            readyToUse = false;
            particles.Play();
            StartCoroutine(Explode());
        }
    }
    
    void Update()
    {
    }
}
