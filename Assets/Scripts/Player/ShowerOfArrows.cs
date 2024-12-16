using UnityEngine;
using System.Collections;

public class ShowerOfArrows : MonoBehaviour
{
    public GameObject arrowPrefab; // Arrow prefab to spawn
    public float damageAmount = 10f; // Damage dealt by arrows
    public float radius = 0.1f; // Radius of the effect (how big the area is)
    public float slowAmount = 0.25f; // Amount to slow enemies down
    public float slowDuration = 3f; // Duration for which the enemy is slowed
    public float spawnInterval = 0.1f; // Interval between each arrow spawn (visual)
    public float height = 10f; // Height from which arrows fall
    public int numberOfArrows = 30; // Number of arrows to spawn during the effect
    private BasePlayer player;

    private void Start()
    {
        player = FindObjectOfType<BasePlayer>();
        // Start the Shower of Arrows effect
        StartCoroutine(ShowerOfArrowsEffect());
        Destroy(gameObject, 3f); // Destroys the object after 5 seconds
    }


    private IEnumerator ShowerOfArrowsEffect()
    {
        ApplyEffectsToEnemies();
        // Show arrows raining down within the area
        for (int i = 0; i < numberOfArrows; i++)
        {
            SpawnArrow();
            yield return new WaitForSeconds(spawnInterval);
        }

        // After the visual effect, apply damage and slow to enemies within the radius

    }

    private void SpawnArrow()
    {
        // Generate a random position within the ring's area
        // float angle = Random.Range(0f, 2f * Mathf.PI);
        // Generate a random position within the ring's area
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(radius*-1f, radius);

        Vector3 spawnPosition = new Vector3(
            transform.position.x + distance * Mathf.Cos(angle),
            transform.position.y + height,
            transform.position.z + distance * Mathf.Sin(angle)
        );
        // Instantiate the arrow and set its destination directly downwards
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        Destroy(arrow, 5f); // Destroy arrow after some time (if not colliding)
    }

    private void ApplyEffectsToEnemies()
    {
        // Find all enemies within the radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    // Deal damage to the enemy
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
