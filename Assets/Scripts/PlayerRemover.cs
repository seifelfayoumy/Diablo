using UnityEngine;

public class PlayerRemover : MonoBehaviour
{
    private void Start()
    {
      GameObject characterInstance = GameObject.FindGameObjectWithTag("Player");
      if(characterInstance != null)
      {
        Destroy(characterInstance);
      }
    }
}