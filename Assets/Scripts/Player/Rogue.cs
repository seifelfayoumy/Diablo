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
    public GameObject arrowPrefab; // Assign the enemy GameObject in the Inspector
    public GameObject smokeBombPrefab; // Assign the enemy GameObject in the Inspector
        public float smokeBombInitialDamage = 10f;
    public float smokeBombDamagePerSecond = 2f;
    public float smokeBombDuration = 5f;
    public GameObject showerArrowsPrefab; // Assign the enemy GameObject in the Inspector

    protected override void Start()
    {
        base.Start();
    }

    // Arrow ability targeting an enemy
    public void Arrow(GameObject enemy)
    {
        animator.SetTrigger("IsArrow");
        audioSource.PlayOneShot(arrowSound);

        //wait for the animation to play
        StartCoroutine(PerformArrow(enemy));
        



        // if (enemy != null)
        // {
        //     Enemy enemyScript = enemy.GetComponent<Enemy>();
        //     if (enemyScript != null)
        //     {
        //         enemyScript.TakeDamage((int)arrowDamage);
        //         playerStats.GainXP(enemyScript.GetXP());
        //     }
        // }
    }

    private IEnumerator PerformArrow(GameObject enemy)
    {
        yield return new WaitForSeconds(2f); // Wait for the animation to reach the point where the arrow is fired

        Vector3 spawnPosition = transform.position; // 2f is the distance in front of the player
        spawnPosition.y = 1f;  // Set Y to 1

        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, transform.rotation);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.Initialize(10, 5, enemy, playerStats);
            Debug.Log("Fireball");
        }
    }


    // Smoke Bomb ability
    public void SmokeBomb(GameObject enemy = null, Vector3? position = null)
    {
        animator.SetTrigger("IsSmoke");
        audioSource.PlayOneShot(smokeBombSound);

        GameObject inferno = Instantiate(smokeBombPrefab, transform.position, Quaternion.identity);
        SmokeBombBehavior infernoBehavior = inferno.GetComponent<SmokeBombBehavior>();
        if (infernoBehavior != null)
        {
            infernoBehavior.Initialize(this, smokeBombInitialDamage, smokeBombDamagePerSecond, smokeBombDuration);
        }

    }

    // Dash ability towards a position
    public void Dash(Vector3 position)
    {
        animator.SetBool("IsDash", true);
        Debug.Log("Dashing");
        audioSource.PlayOneShot(dashSound);

        // Implement charging logic
        StartCoroutine(PerformDash(position));
    }

    private IEnumerator PerformDash(Vector3 position)
    {
        float originalSpeed = navMeshAgent.speed;
        navMeshAgent.speed = 10f; // Increased speed for charge

        // Adjust rotation
        Vector3 lookAt = new Vector3(position.x, transform.position.y, position.z);
        transform.LookAt(lookAt);
        navMeshAgent.SetDestination(position);


        // Move towards the target position, checking both animation and movement completion
        while (true)
        {

            // Check if the animation is finished
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break; // Animation is finished
            }

            // Check if the agent has reached the destination (taking stopping distance into account)
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
            {
                break; // Agent has completed its path
            }

            // If neither condition is met, continue waiting
            yield return null;
        }



        // Set the Charge bool to false and reset speed
        animator.SetBool("IsDash", false);
        navMeshAgent.speed = originalSpeed;
    }

    // Shower of Arrows ability at a position
    public void ShowerOfArrows(Vector3 position)
    {
        animator.SetTrigger("IsShower");
        audioSource.PlayOneShot(showerOfArrowsSound);

        // Implement area damage and slow effect
        GameObject shower = Instantiate(showerArrowsPrefab, position, Quaternion.identity);

        // Collider[] hitColliders = Physics.OverlapSphere(position, 5f); // Example range
        // foreach (var hit in hitColliders)
        // {
        //     if (hit.CompareTag("Enemy"))
        //     {
        //         Enemy enemyScript = hit.GetComponent<Enemy>();
        //         if (enemyScript != null)
        //         {
        //             enemyScript.TakeDamage((int)showerDamage);
        //             enemyScript.Slow(0.25f, 3f); // Assuming Enemy class has a Slow method
        //             playerStats.GainXP(enemyScript.GetXP());
        //         }
        //     }
        // }
    }
}
