// Rogue.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Rogue : BasePlayer
{
    // Rogue-specific abilities
    public AudioClip dashSound;
    public AudioClip arrowSound;
    public AudioClip smokeBombSound;
    public AudioClip showerOfArrowsSound;
    public float arrowDamage = 5f;
    public float showerDamage = 10f;
    public float dashSpeed = 10f;

    protected override void Start()
    {
        base.Start();
    }

    // Arrow ability targeting an enemy
    public void Arrow(GameObject enemy)
    {
        animator.SetTrigger("IsArrow");
        audioSource.PlayOneShot(arrowSound);

        if (enemy != null)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage((int)arrowDamage);
                playerStats.GainXP(enemyScript.GetXP());
            }
        }
    }

    // Smoke Bomb ability
    public void SmokeBomb(GameObject enemy = null, Vector3? position = null)
    {
        animator.SetTrigger("IsSmoke");
        audioSource.PlayOneShot(smokeBombSound);

        // Implement smoke bomb effect: stun enemies within range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f); // Example range
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemyScript = hit.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.Stun(5f); // Assuming Enemy class has a Stun method
                }
            }
        }
    }

    // Dash ability towards a position
    public void Dash(Vector3 position)
    {
        animator.SetTrigger("Dash");
        audioSource.PlayOneShot(dashSound);

        StartCoroutine(PerformDash(position));
    }

    private IEnumerator PerformDash(Vector3 position)
    {
        float originalSpeed = navMeshAgent.speed;
        navMeshAgent.speed = dashSpeed;

        navMeshAgent.SetDestination(position);
        while (navMeshAgent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        navMeshAgent.speed = originalSpeed;
    }

    // Shower of Arrows ability at a position
    public void ShowerOfArrows(Vector3 position)
    {
        animator.SetTrigger("IsShower");
        audioSource.PlayOneShot(showerOfArrowsSound);

        // Implement area damage and slow effect
        Collider[] hitColliders = Physics.OverlapSphere(position, 5f); // Example range
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemyScript = hit.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage((int)showerDamage);
                    enemyScript.Slow(0.25f, 3f); // Assuming Enemy class has a Slow method
                    playerStats.GainXP(enemyScript.GetXP());
                }
            }
        }
    }
}
