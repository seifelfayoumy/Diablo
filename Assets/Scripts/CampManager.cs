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
        Debug.Log(enemiesRemaining);
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            SpawnRuneFragment();
        }
        Debug.Log("new:" + enemiesRemaining);

    }

    void SpawnRuneFragment()
    {
        Instantiate(runeFragmentPrefab, runeFragmentSpawnPoint.position, Quaternion.identity);
        Debug.Log("All enemies cleared! Rune Fragment spawned.");
    }
}
