using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding is tagged as "Sword"
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("SWORD WORKS");
            // Get the player component from the collision object (player should be a separate GameObject)
            BasePlayer playerScript = collision.gameObject.GetComponent<BasePlayer>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(30);
                Debug.Log("Player hit by sword for 30 damage.");
            }
        }
    }
}
    
