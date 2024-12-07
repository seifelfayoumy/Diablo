using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Barbarian : BasePlayer
{
    // Ability-specific variables
    public AudioClip shieldSound;
    public AudioClip bashSound;
    public AudioClip chargeSound;
    public AudioClip ironMaelstormSound;
    public float bashDamage = 15f;
    public float ironMaelstormDamage = 10f;
    public GameObject shieldObject;

    protected override void Start()
    {
        base.Start();
    }

    // Bash ability targeting an enemy
    public void Bash(GameObject enemy)
    {
        animator.SetTrigger("Bash");
        audioSource.PlayOneShot(bashSound);

        if (enemy != null)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage((int)bashDamage);
                playerStats.GainXP(enemyScript.GetXP());
            }
        }
    }

    // Shield ability
    public void Shield(GameObject enemy = null, Vector3? position = null)
    {
        animator.SetTrigger("Shield");
        audioSource.PlayOneShot(shieldSound);

        // Spawn and activate shield
        GameObject shield = Instantiate(shieldObject, transform.position, Quaternion.identity);
        shield.transform.SetParent(transform);
        Destroy(shield, 3f);
    }

    // Iron Maelstorm ability
    public void IronMaelstorm(GameObject enemy = null, Vector3? position = null)
    {
        animator.SetTrigger("IronMaelstorm");
        audioSource.PlayOneShot(ironMaelstormSound);

        // Area damage around the player
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f); // Example range
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemyScript = hit.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage((int)ironMaelstormDamage);
                    playerStats.GainXP(enemyScript.GetXP());
                }
            }
        }
    }

    // Charge ability towards a position
    public void Charge(Vector3 position)
    {
        animator.SetTrigger("Charge");
        audioSource.PlayOneShot(chargeSound);

        // Implement charging logic
        StartCoroutine(PerformCharge(position));
    }

    private IEnumerator PerformCharge(Vector3 position)
    {
        float originalSpeed = navMeshAgent.speed;
        navMeshAgent.speed = 10f; // Increased speed for charge

        navMeshAgent.SetDestination(position);
        while (navMeshAgent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        navMeshAgent.speed = originalSpeed;
        // Implement damage to enemies in the path if needed
    }
}
