using UnityEngine;

public class CampManager : MonoBehaviour
{
    public int totalEnemies;
    private int enemiesRemaining;
    public GameObject runeFragmentPrefab;
    public Transform runeFragmentSpawnPoint;

    void Start()
    {
        enemiesRemaining = totalEnemies;
    }

    public void EnemyDied()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            SpawnRuneFragment();
        }
    }

    void SpawnRuneFragment()
    {
        Instantiate(runeFragmentPrefab, runeFragmentSpawnPoint.position, Quaternion.identity);
        Debug.Log("All enemies cleared! Rune Fragment spawned.");
    }
}
