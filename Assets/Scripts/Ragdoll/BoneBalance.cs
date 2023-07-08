using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBalance : MonoBehaviour
{
    public BoneBalanceConfig balanceConfig;
    public bool debugDowntime = false;
    private Rigidbody2D rb;

    private float downtime = 0f;
    private float getUpForce = 0f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        downtime = balanceConfig.getUpTime;
        getUpForce = balanceConfig.maxForce;
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.magnitude < balanceConfig.velocityThreshold)
        {
            downtime += Time.deltaTime;
            if(downtime > balanceConfig.getUpTime)
            {
                if(getUpForce < balanceConfig.maxForce)
                    getUpForce += balanceConfig.getUpSpeed * Time.deltaTime;
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, balanceConfig.targetRotation, getUpForce * Time.deltaTime));
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
