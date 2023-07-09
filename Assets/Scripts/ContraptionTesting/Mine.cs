using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Mine : ContraptionBase
{
    public float explosionTime = 0.2f;
    public float basePower = 30.0f;

    public Color readyColor;
    public Color usedColor;

    public ParticleSystem particles;
    
    private Vector2 position;

    private Vector2 actualExplosionPositionWithOffsetFromRotation;

    private bool readyToUse;
    
    void Start()
    {
        position = new Vector2(this.transform.position.x, this.transform.position.y);
        readyToUse = true;
    }

    private void ApplyForce(Rigidbody2D limb, float time)
    {
        var distance = (limb.position - actualExplosionPositionWithOffsetFromRotation).magnitude;
        var forceDirection = (limb.position - actualExplosionPositionWithOffsetFromRotation).normalized;

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
            Vector2 offsetDir = new Vector2();
            switch (rot)
            {
                case 0:
                    offsetDir = new Vector2(0, -1);
                    break;
                case 1:
                    offsetDir = new Vector2(1, 0);
                    break;
                case 2:
                    offsetDir = new Vector2(0, 1);
                    break;
                case 3:
                    offsetDir = new Vector2(-1, 0);
                    break;
            }


            float offsetDist = FindObjectOfType<Level>().GetComponentInChildren<Tilemap>().cellSize.x / 2;
            Vector2 offset = offsetDir * offsetDist;
            actualExplosionPositionWithOffsetFromRotation = position + offset;

            particles.gameObject.transform.SetLocalPositionAndRotation(new Vector2(0, -offsetDist), Quaternion.identity);

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
