using UnityEngine;
using System.Collections;

public class ShowerOfArrows : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float damageAmount = 10f;
    public float radius = 0.1f;
    public float slowAmount = 0.25f;
    public float slowDuration = 3f;
    public float spawnInterval = 0.1f;
    public float height = 10f;
    public int numberOfArrows = 30;
    private BasePlayer player;

    private void Start()
    {
        player = FindObjectOfType<BasePlayer>();
        StartCoroutine(ShowerOfArrowsEffect());
        Destroy(gameObject, 3f);
    }


    private IEnumerator ShowerOfArrowsEffect()
    {
        ApplyEffectsToEnemies();
        for (int i = 0; i < numberOfArrows; i++)
        {
            SpawnArrow();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnArrow()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(radius*-1f, radius);

        Vector3 spawnPosition = new Vector3(
            transform.position.x + distance * Mathf.Cos(angle),
            transform.position.y + height,
            transform.position.z + distance * Mathf.Sin(angle)
        );
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        Destroy(arrow, 5f);
    }

    private void ApplyEffectsToEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)damageAmount);
                    enemy.Slow(slowAmount, slowDuration);

                    if(enemy.GetHP() <= 0)
                        player.playerStats.GainXP(enemy.GetXP());
                }
            }
        }
    }

    private IEnumerator SlowEnemy(Enemy enemy)
    {
        enemy.Slow(slowAmount, slowDuration);
        yield return new WaitForSeconds(slowDuration);
    }
}
