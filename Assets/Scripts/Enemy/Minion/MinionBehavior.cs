using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MinionEnemy : Enemy
{
    public Animator animator;
    public float detectionRange = 10f;
    public float moveSpeed = 1f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;
    public bool isAlerted;
    public Transform player; // Reference to the player's transform

    public Vector3 CampPosition; // Store the enemy's original position

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
                animator.SetBool("IsWalking", true); // Trigger moving animation

            }
            else
            {
             
                animator.SetBool("IsWalking", false); // Stop moving animation when not chasing
            }

            // Check if the player is within attack range
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("Attack");
                // Update the time of the last attack
                lastAttackTime = Time.time;
            }
        }


    }
}
