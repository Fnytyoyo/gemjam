using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollMenu : MonoBehaviour
{
    public float force = 40;
    public float angularForce = 400;
    private Rigidbody2D rb;

    private float currCD = 0f;
    private float maxCD = 6f;
    private float currXdir = 1f;
    private Vector2 forceVec = new Vector2(0.44721f, 0.89443f);

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        currCD = 4f;
    }


    void FixedUpdate()
    {
        currCD += Time.deltaTime;

        if (currCD > maxCD)
        {
            currCD = 0f;
            Vector2 vec = new Vector2(forceVec.x * currXdir, forceVec.y);
            rb.AddForce(vec * force, ForceMode2D.Impulse);
            rb.AddTorque(angularForce * Mathf.Deg2Rad * rb.inertia, ForceMode2D.Impulse);

            currXdir *= -1f;
        }
    }
}
