using UnityEngine;

public class RuneFragment : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
            //PlayerStats playerStats = other.GetComponent<PlayerStats>();
            //if (playerStats != null)
            //{
               // playerStats.runeFragments++; // Increment rune fragment count
             //   Debug.Log("Rune Fragment Collected! Total: " + playerStats.runeFragments);
           // }

            // Destroy or deactivate the fragment after collection
         //   Destroy(gameObject);
       // }
    }
}