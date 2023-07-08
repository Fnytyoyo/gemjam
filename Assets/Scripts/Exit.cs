using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    bool hasTriggered;
    // Start is called before the first frame update
    void Start()
    {
        hasTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hasTriggered) // Trigger only once on this level
        {
            return;
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Ragdoll"))
        {
            hasTriggered = true;

            FindObjectOfType<GameMode>().NextLevel();
        }
    }
}
