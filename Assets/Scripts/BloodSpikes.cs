using System.Collections;
using UnityEngine;

public class BloodSpikes : MonoBehaviour
{
    public GameObject[] spikes;  // Array of 6 spikes to be activated
    public float activationDelay = 0.5f; // Delay between activating each spike
    public float destroyDelay = 3f; // Delay before destroying the BloodSpikes object after activation

    private void Start()
    {
        // Start the spike activation process
 // Ensure there are exactly 6 spikes

            StartCoroutine(ActivateSpikes());

    }

    private IEnumerator ActivateSpikes()
    {
        // Activate each spike with a delay between activations
        for (int i = 0; i < spikes.Length; i++)
        {
            // Activate the current spike
            spikes[i].SetActive(true);
            
            // Wait for the specified activation delay
            yield return new WaitForSeconds(0.4f);
        }

        // After activating all spikes, destroy the BloodSpikes object after a short delay
        Destroy(gameObject, 3f);
    }
}
