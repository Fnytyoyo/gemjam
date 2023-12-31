using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Mine : ContraptionBase
{
    public GameObject particlesPrefab;
    public GameObject particlesPos;
    public AudioSource audioSource;
    
    public float explosionTime = 0.2f;
    public float basePower = 30.0f;

    public float maxRange = 4.0f;
    public float FalloffCoeff = 0.6f;

    public Color readyColor;
    public Color usedColor;

    private Vector2 position;

    private Vector2 actualExplosionPositionWithOffsetFromRotation;

    private bool tileHidden;
    private bool readyToUse;

    private Matrix4x4 savedTileMatrix;

    void Start()
    {
        position = new Vector2(this.transform.position.x, this.transform.position.y);
        audioSource = GetComponent<AudioSource>();
        readyToUse = true;
        tileHidden = false;
    }

    public override void OnRecharge()
    {
        ShowThisTile();
        readyToUse = true;
    }

    private void ApplyForce(Rigidbody2D limb)
    {
        var distance = (limb.position - actualExplosionPositionWithOffsetFromRotation).magnitude;
        var forceDirection = (limb.position - actualExplosionPositionWithOffsetFromRotation).normalized;
        
        var distanceModifier = distance > maxRange ? 0 : 1 / (1 + Mathf.Exp(FalloffCoeff * maxRange * (distance - maxRange / 2)));

        Debug.Log($"Distance: {distance}; DistMod: {distanceModifier}");

        limb.AddForce(forceDirection * basePower * distanceModifier,  ForceMode2D.Impulse);
    }
    
    private void ApplyForces()
    {
        foreach (var limb in Limbs.limbRigidBodies)
        {
            ApplyForce(limb);
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

            //particles.gameObject.transform.SetLocalPositionAndRotation(new Vector2(0, -offsetDist), Quaternion.identity);

            Debug.Log("Mine explosion");
            readyToUse = false;
            
            GameObject go = Instantiate(particlesPrefab, new Vector3(particlesPos.transform.position.x, particlesPos.transform.position.y, 0), Quaternion.identity);
            go.GetComponent<AudioSource>().Play();
            
            ApplyForces();

            if (wasBuiltByPlayer == true)
            {
                var dispatcher = FindObjectOfType<ContraptionInteractionDispatcher>();
                dispatcher.RemoveBuilding(transform.position);
            }
            else
            { 
                HideThisTile();   
            }
        }
    }
    
    void Update()
    {
    }

    Tilemap GetParentTilemap()
    {
        var currLevelObj = FindObjectOfType<Level>();
        return currLevelObj.contraptionTilemap;
    }

    void HideThisTile()
    {
        if (tileHidden == false)
        {
            var tm = GetParentTilemap();
            var coords = tm.WorldToCell(transform.position);

            savedTileMatrix = tm.GetTransformMatrix(coords);

            tm.SetTile(coords, null);

            tileHidden = true;
        }
    }

    void ShowThisTile()
    {
        if (tileHidden == true)
        {
            var tm = GetParentTilemap();
            var coords = tm.WorldToCell(transform.position);

            tm.SetTile(coords, tile);
            tm.SetTransformMatrix(coords, savedTileMatrix);

            tileHidden = false;
        }
    }
}
