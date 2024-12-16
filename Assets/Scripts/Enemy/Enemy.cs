using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHP;
    public int currentHP;
    public int xpReward;
    public CampManager campManager;
    public HealthBar healthBar;
    protected bool isStunned = false;
    private bool isSlowed = false;
    public bool isInvincible = false;
    protected float originalSpeed = 3f;
    protected NavMeshAgent navMeshAgent;
    public bool isAlerted = false;

    public AudioManager audioManager;

    protected virtual void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
        currentHP = maxHP;
        if (healthBar != null)
        {
            Debug.Log("Setting max HP: " + maxHP);
            healthBar.SetMaxHealth(maxHP);
            healthBar.SetHealth(currentHP);
            healthBar.SetHealthText(currentHP);
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        Debug.Log(navMeshAgent.speed + " IS THE SPEED");
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = originalSpeed;
        }
    }

    protected virtual void Update()
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHP);
            healthBar.SetHealthText(currentHP);
            healthBar.SetMaxHealth(maxHP);
        }
    }

    public virtual void TakeDamage(int damage)
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
            if (currentHP <= 0)
            {
                Die();
            }
            else
            {
                animator.SetTrigger("Reaction");
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

    public void Die()
    {
        Collider collider = this.gameObject.GetComponent<Collider>();
        if(collider.enabled == false)
        {
            return;
        }
        if (campManager != null)
        {
            campManager.EnemyDied();
        }
        animator.SetTrigger("IsDead");
        audioManager.PlaySFX(audioManager.enemyDiesSFX);
        
        collider.enabled = false;

        if(isAlerted)
        {
            AlertNearbyEnemy();
        }


        Destroy(gameObject, 2.5f);
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
        animator.SetBool("IsStunned", true);
        yield return new WaitForSeconds(duration);
        animator.SetBool("IsStunned", false);
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
        yield return new WaitForSeconds(duration);
        originalSpeed = OldSpeed;
        isSlowed = false;
    }

    void AlertNearbyEnemy()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, 20);

        foreach (var collider in nearbyEnemies)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy otherEnemy = collider.GetComponent<Enemy>();
                if (otherEnemy != null && !otherEnemy.isAlerted)
                {
                    otherEnemy.isAlerted = true;
                    Debug.Log(gameObject.name + " alerted " + otherEnemy.gameObject.name);
                    break;
                }
            }
        }
    }
}
