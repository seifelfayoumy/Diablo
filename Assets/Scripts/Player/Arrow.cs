using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed;
    private float damage;
    private GameObject targetEnemy;
    private PlayerStats playerStats;
    
    private Rigidbody rb;

    // Method to initialize the fireball properties
    public void Initialize(float fireballSpeed, float fireballDamage, GameObject enemy, PlayerStats stats)
    {
        speed = fireballSpeed;
        damage = fireballDamage;
        targetEnemy = enemy;
        playerStats = stats;

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed; // Set initial velocity in the direction the fireball is facing
    }

    // Update is called once per frame
    // void Update()
    // {
    //     // Rotate the fireball to face the direction of movement
    //     //move forward
    //     transform.position += transform.forward * speed * Time.deltaTime;
    // }

    private void OnCollisionEnter(Collision other)
    {
        // if (other.CompareTag("Enemy"))
        // {
        //     // Apply damage to the enemy
        //     Enemy enemyScript = other.GetComponent<Enemy>();
        //     if (enemyScript != null)
        //     {
        //         enemyScript.TakeDamage((int)damage);
        //         playerStats.GainXP(enemyScript.GetXP());
        //     }

        //     // Destroy the fireball after hitting an enemy
        //     Destroy(gameObject);
        // }
        // else if (other.CompareTag("Obstacle"))
        {
            // Destroy fireball if it hits an obstacle (optional)
            Destroy(gameObject);
        }
    }
}
