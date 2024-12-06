// Enemy.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int maxHP = 20;
    public int currentHP;
    public int xpReward = 10;

    public HealthBar healthBar; // Reference to a HealthBar UI component

    private bool isStunned = false;
    private bool isSlowed = false;
    private float originalSpeed;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
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
    }

    void Update()
    {
        // Enemy AI logic (e.g., chasing the player) goes here
        // For example:
        // if not stunned, chase the player
    }

    public void TakeDamage(int damage)
    {
        if (isStunned) // Assuming enemies can't take damage while stunned
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
        // Handle death, e.g., play death animation, disable AI, drop loot, etc.
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
        // Optional: Play stun animation or effect
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
