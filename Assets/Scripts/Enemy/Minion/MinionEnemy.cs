using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MinionEnemy : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public int xpReward;

    public HealthBar healthBar; // Reference to a HealthBar UI component

    private bool isStunned = false;
    private bool isSlowed = false;
    public bool isAltered;
    private float originalSpeed;
    private NavMeshAgent navMeshAgent;
    public Transform player; // Reference to the player's transform

    public Vector3 CampPosition; // Store the enemy's original position
    private bool canAttack = true;

    void Start()
    {
        isAltered = Random.Range(0, 2) == 0;
        currentHP = maxHP;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHP);
            healthBar.SetHealth(currentHP);
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            originalSpeed = navMeshAgent.speed;
        }

        // Store the original camp position
        CampPosition = transform.position;
    }

    void Update()
    {
        if (isStunned)
            return;

        // Ensure the player is assigned
        if (player == null)
        {
            Debug.LogWarning("Player transform is not assigned!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isAltered)
        {
            if (distanceToPlayer <= 10f) // Chase range
            {
                navMeshAgent.SetDestination(player.position);

                if (distanceToPlayer <= 2f) // Attack range
                {
                    Attack(player.gameObject);
                }
            }
            else
            {
                // Return to camp position
                navMeshAgent.SetDestination(CampPosition);
            }
        }
        else
        {
            // Stay idle at camp
            navMeshAgent.SetDestination(CampPosition);
        }
    }

    void Attack(GameObject player)
    {
        if (!canAttack) return;

        canAttack = false;
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(5); // Minion's attack damage
        }

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1.5f); // Attack cooldown time
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        if (isStunned)
            return;

        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHP);
        }
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public int GetXP()
    {
        return xpReward;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void Stun(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine(duration));
        }
    }

    IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    public void Slow(float multiplier, float duration)
    {
        if (!isSlowed && navMeshAgent != null)
        {
            StartCoroutine(SlowCoroutine(multiplier, duration));
        }
    }

    IEnumerator SlowCoroutine(float multiplier, float duration)
    {
        isSlowed = true;
        navMeshAgent.speed *= multiplier;
        yield return new WaitForSeconds(duration);
        navMeshAgent.speed = originalSpeed;
        isSlowed = false;
    }
}
