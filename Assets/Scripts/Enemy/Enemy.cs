// Enemy.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public int xpReward;
    public CampManager campManager;


    public HealthBar healthBar; // Reference to a HealthBar UI component

    protected bool isStunned = false;
    private bool isSlowed = false;
    public bool isInvincible = false;
    protected float originalSpeed = 3f;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        currentHP = maxHP;
        if (healthBar != null)
        {
            Debug.Log("Setting max HP: " + maxHP);
            healthBar.SetMaxHealth(maxHP);
            healthBar.SetHealth(currentHP);
            healthBar.SetHealthText(currentHP);
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            originalSpeed = navMeshAgent.speed;
        }
    }

  protected virtual void Update()
  {
    // Enemy AI logic (e.g., chasing the player) goes here
    // For example:
    // if not stunned, chase the player
    // Debug.Log(isInvincible + " IS THE CONDISTION");


    if (healthBar != null)
    {
      healthBar.SetHealth(currentHP);
      healthBar.SetHealthText(currentHP);
      healthBar.SetMaxHealth(maxHP);

    }
  }



    public void TakeDamage(int damage)
    {
        Debug.Log("TAKING DAMAGE ON: " + damage);
        Debug.Log("Setting isInvincible on: " + this.gameObject.name);

        if (isInvincible == true)
        {
            Debug.Log("Enemy is Invincible");
        }

        else
        {
            Debug.Log("Cond: " + isInvincible);
            Debug.Log("Enemy is NOT Invincible");
            currentHP -= damage;
            currentHP = Mathf.Max(0, currentHP);
            Debug.Log(currentHP + " OF ENEMY");
            if (healthBar != null)
            {
                // healthBar.SetHealth(currentHP);
                // healthBar.SetHealthText(currentHP);
            }
            if (currentHP <= 0)
            {
                Die();
            }
        }
    }

    public int GetHP()
    {
        return currentHP;
    }

    public int GetXP()
    {
        return xpReward;
    }

    void Die()
    {
        if (campManager != null)
        {
            campManager.EnemyDied();
        }
        Destroy(gameObject);
    }


    public void Stun(float duration)
    {
        if (!isStunned)
        {
            Debug.Log("Stunned for: " + duration);
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
        if (!isSlowed)
        {
            Debug.Log("Slowed for: " + duration);
            Debug.Log("Speed: " + multiplier);
            StartCoroutine(SlowCoroutine(multiplier, duration));
        }
    }

    IEnumerator SlowCoroutine(float multiplier, float duration)
    {
        isSlowed = true;
        float OldSpeed = originalSpeed;
        originalSpeed *= multiplier;
        // originalSpeed = navMeshAgent.speed;
        yield return new WaitForSeconds(duration);
        originalSpeed = OldSpeed;
        isSlowed = false;
    }
}
