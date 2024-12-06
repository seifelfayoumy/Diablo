using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle enemy death (e.g., play death animation, destroy object)
        Debug.Log("Enemy has died!");
        Destroy(gameObject); // Destroy the enemy object
    }
}
