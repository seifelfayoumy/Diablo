using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("SWORD WORKS");
            BasePlayer playerScript = collision.gameObject.GetComponent<BasePlayer>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(30);
                Debug.Log("Player hit by sword for 30 damage.");
            }
        }
    }
}
    
