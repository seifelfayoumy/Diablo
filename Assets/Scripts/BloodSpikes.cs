using System.Collections;
using UnityEngine;

public class BloodSpikes : MonoBehaviour
{
    public GameObject[] spikes;
    public float activationDelay = 0.5f;
    public float destroyDelay = 3f;

    private void Start()
    {
            StartCoroutine(ActivateSpikes());
    }

    private IEnumerator ActivateSpikes()
    {
        for (int i = 0; i < spikes.Length; i++)
        {
            spikes[i].SetActive(true);
            
            yield return new WaitForSeconds(0.4f);
        }

        Destroy(gameObject, 3f);
    }
}
