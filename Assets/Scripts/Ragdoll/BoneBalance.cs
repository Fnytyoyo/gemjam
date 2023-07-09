using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBalance : MonoBehaviour
{
    public BoneBalanceConfig balanceConfig;
    public bool isTorso = false;
    public bool debugDowntime = false;
    private Rigidbody2D rb;

    private float downtime = 0f;
    private float getUpForce = 0f;

    private Vector2 pos_vec = Vector2.zero;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        downtime = balanceConfig.getUpTime;
        getUpForce = balanceConfig.maxForce;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rb.velocity.magnitude < balanceConfig.velocityThreshold)
        {
            downtime += Time.deltaTime;
            if(downtime > balanceConfig.getUpTime)
            {
                if(getUpForce < balanceConfig.maxForce)
                    getUpForce += balanceConfig.getUpSpeed * Time.deltaTime;
                var rotation = rb.rotation;
                rb.MoveRotation(Mathf.LerpAngle(rotation, balanceConfig.targetRotation, getUpForce * Time.deltaTime));
                if (isTorso && balanceConfig.swiftGetUp)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, LayerMask.GetMask("Floor"));
                    if (hit.collider != null)
                    {
                        float targetHeight = hit.point.y + balanceConfig.bodyHeight;
                        float pos_y = Mathf.Lerp(rb.position.y, targetHeight + balanceConfig.bodyHeightBias,
                            Mathf.Abs(targetHeight + balanceConfig.bodyHeightBias - rb.position.y) * balanceConfig.bodyMoveStrength * Time.deltaTime);
                        pos_vec.Set(rb.position.x, pos_y);
                        if(rb.position.y < targetHeight)
                            rb.MovePosition(pos_vec);
                    }
                }
            }            
        }
        else
        {
            getUpForce = 0;
            downtime -= rb.velocity.magnitude;
            downtime = Mathf.Max(0f, downtime);
        }
        if(debugDowntime)
        {
            Debug.Log("Downtime: " + downtime + ", Force: " + getUpForce + ", Velocity: " + rb.velocity.magnitude);
        }

    }
}
