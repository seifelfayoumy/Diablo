using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject barbarianPrefab;
    public GameObject sorcererPrefab;
    public GameObject roguePrefab;

    public void SelectCharacter(string characterClass)
    {
        switch (characterClass)
        {
            case "Barbarian":
                Instantiate(barbarianPrefab);
                break;
            case "Sorcerer":
                Instantiate(sorcererPrefab);
                break;
            case "Rogue":
                Instantiate(roguePrefab);
                break;
            default:
                Debug.LogError("Invalid Character Class");
                break;
        }
    }
}
