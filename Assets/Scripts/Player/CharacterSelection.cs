using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject barbarianPrefab;
    public GameObject roguePrefab;
    public GameObject sorcererPrefab;
    public Transform spawnPoint;

    public void SelectBarbarian()
    {
        Instantiate(barbarianPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void SelectRogue()
    {
        Instantiate(roguePrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void SelectSorcerer()
    {
        Instantiate(sorcererPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
