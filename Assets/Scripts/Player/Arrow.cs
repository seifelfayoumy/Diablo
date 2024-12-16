using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed;
    private float damage;
    private GameObject targetEnemy;
    private PlayerStats playerStats;
    
    private Rigidbody rb;

    public void Initialize(float fireballSpeed, float fireballDamage, GameObject enemy, PlayerStats stats)
    {
        speed = fireballSpeed;
        damage = fireballDamage;
        targetEnemy = enemy;
        playerStats = stats;

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
