using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spikes : ContraptionBase
{
    public ParticleSystem particles;
    
    public float maxDistance;
    public float forcePower;

    private bool active = false;
    
    private void Start()
    {
    }

    private void ApplyForce(Rigidbody2D limb, Vector2 forceDirection)
    {
        float distance = forceDirection.magnitude;
        float normalizedDistance = distance / maxDistance;
        float distanceModifier = -1 / (1 + Mathf.Exp(-30 * (normalizedDistance - 0.8f))) + 1;
        
        limb.AddForce(forceDirection.normalized * (forcePower * distanceModifier));
    }
    
    private void ApplyForces()
    {
        foreach (var limb in Limbs.limbRigidBodies)
        {
            var direction3D = this.transform.position - limb.gameObject.transform.position;
            var direction2D = new Vector2(direction3D.x, direction3D.y);
            if (direction2D.magnitude < maxDistance)
            {
                ApplyForce(limb, direction2D);
            }
            
        }
    }
    
    public override void OnInteract(Vector3 pos, int rotation)
    {
        active = !active;

        if (active)
        {
            particles.Play();
        }
        else
        {
            particles.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (active)
        {
            ApplyForces();
        }
    }
}
