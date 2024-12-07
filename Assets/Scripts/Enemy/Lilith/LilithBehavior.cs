using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithBehavior : Enemy
{
    private Animator animator;
    private bool isCombatStarted = false;
    private Transform player; // Reference to the player's transform

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Find the player GameObject
        StartCoroutine(DiveBombRoutine()); // Start the divebomb coroutine
    }

    void Update()
    {
        // Make Lilith look at the player if it exists
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Keep the y-axis the same to prevent tilting up/down
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Adjust rotation speed as needed
        }
    }

    // Coroutine for the divebomb action every 5 seconds
    private IEnumerator DiveBombRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // Wait for 5 seconds
            if (animator != null)
            {
                animator.SetTrigger("DiveBomb"); // Trigger the divebomb animation
            }
        }
    }

    void OnCollisionEnter(Collider other)
    {
        Debug.Log($"Collider entered: {other.name}");

        if (other.CompareTag("Axe") && !isCombatStarted)
        {
           // Debug.Log(base.health.ToString());
            isCombatStarted = true;
            //base.health = base.health - 5;
            // Debug.Log("Lilith Die");
            // animator.SetTrigger("Die");
        }
    }
}
