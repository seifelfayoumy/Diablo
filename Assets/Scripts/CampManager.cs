using UnityEngine;

public class CampManager : MonoBehaviour
{
    public int totalEnemies;
    private int enemiesRemaining;
    public GameObject runeFragmentPrefab;
    public Transform runeFragmentSpawnPoint;
    bool isSpawned = false;

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
    }

    void SpawnRuneFragment()
    {
        if(isSpawned)
            return;
        isSpawned = true;
        Instantiate(runeFragmentPrefab, runeFragmentSpawnPoint.position, Quaternion.identity);
    }
}
