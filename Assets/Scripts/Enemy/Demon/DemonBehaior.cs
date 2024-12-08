using System.Collections;
using UnityEngine;

public class DemonBehavior : Enemy
{
    public Animator animator; // Attach the Animator component
    public Transform player; // Reference to the player's transform
    public float attackRange = 5f; // Range to start attacking
    public float detectionRange = 10f; // Range to detect the player
    public float moveSpeed = 3f; // Speed at which the Demon moves towards the player
    public float attackCooldown = 2f; // Cooldown time between attacks
    private int countA = 0; // Counter to track attack sequences
    private float lastAttackTime = 0f; // Time of the last attack
    public GameObject sword; // Reference to the sword object
    void Start()
    {
        animator = GetComponent<Animator>();

        // Find the player by tag, ensure player exists
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Face the player
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Prevent rotation in the y-axis
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Check if the player is within detection range
            if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
            {
                // Move towards the player
                transform.position += direction * moveSpeed * Time.deltaTime;
                animator.SetBool("Iswalking", true); // Trigger moving animation

            }
            else
            {
             
                animator.SetBool("Iswalking", false); // Stop moving animation when not chasing
            }

            // Check if the player is within attack range
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                if (countA < 2)
                {
                   // sword.SetActive(true);
                    Debug.Log("COUNT " + countA);
                    animator.SetTrigger("StartAttack"); // Trigger sword attack
                    countA++;
                }
                else
                {
                    //sword.SetActive(false);
                    Debug.Log("COUNT " + countA);
                    animator.SetTrigger("Throw"); // Trigger throw attack
                    countA = 0; // Reset count
                }

                // Update the time of the last attack
                lastAttackTime = Time.time;
            }
        }


    }
}