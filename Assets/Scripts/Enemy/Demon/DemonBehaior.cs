using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemonBehavior : Enemy
{
  public Transform player; // Reference to the player's transform
  public Transform playerClone; // Reference to the player's transform
  public float attackRange = 5f; // Range to start attacking
  public float detectionRange = 10f; // Range to detect the player
  public float moveSpeed = 3f; // Speed at which the Demon moves towards the player
  public float attackCooldown = 2f; // Cooldown time between attacks
  private int countA = 0; // Counter to track attack sequences
  private float lastAttackTime = 0f; // Time of the last attack
  public GameObject sword; // Reference to the sword object

    public float radius = 5f;      // Radius of the circular path
    private float angle = 0f;  // This will track the agent's position around the circle


  public Vector3 spawnPosition;

  protected override void Start()
  {
    base.Start();
    animator = GetComponent<Animator>();
    spawnPosition = this.transform.position;
    attackCooldown = 5f; // Cooldown time between attacks

    // Find the player by tag, ensure player exists
    player = GameObject.FindGameObjectWithTag("Player")?.transform;
  }

  protected override void Update()
  {
    base.Update();
    moveSpeed = originalSpeed;
    if (isStunned)
    {
      return;
    }

    if (base.currentHP <= 0)
    {
      return;
    }


    if (!isAlerted && SceneManager.GetActiveScene().name == "MainLevel")
    {
      animator.SetBool("Iswalking", true); // Stop moving animation when not chasing
      angle += originalSpeed * Time.deltaTime / radius;  // speed here is the distance per second along the circle's edge
      if (angle >= 360f) angle -= 360f;  // Keep the angle within 0-360 degrees

      // Calculate the new position around the circle
      Vector3 newPos = campManager.transform.position + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);

      // Move the NavMeshAgent to the new position
      navMeshAgent.destination = newPos;

      return;
    }
    // if(isAlerted){

    

    playerClone = GameObject.FindGameObjectWithTag("PlayerClone")?.transform;
    if (playerClone != null)
    {
      player = playerClone;
    }
    else
    {
      player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    if (player != null)
    {
      // Calculate distance to the player
      float distanceToPlayer = Vector3.Distance(transform.position, player.position);

      // Face the player
      Vector3 direction = (player.position - transform.position).normalized;
      direction.y = 0; // Prevent rotation in the y-axis
      Quaternion lookRotation = Quaternion.LookRotation(direction);
      transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

      if (SceneManager.GetActiveScene().name == "MainLevel")
      {
        // Calculate distance to the player from Campfire
        float distanceToPlayerFromCampfire = Vector3.Distance(campManager.runeFragmentSpawnPoint.position, player.position);
        float distanceToSpawnPosition = Vector3.Distance(transform.position, spawnPosition);

        // Check if the player is within detection range
        if (distanceToPlayerFromCampfire <= detectionRange && distanceToPlayer > attackRange)
        {
          // Move towards the player
          navMeshAgent.destination = player.position;
          animator.SetBool("Iswalking", true); // Trigger moving animation
        }
        else
        {
        //   if (distanceToSpawnPosition > 1)
        //   {
            // // Face the spawn position
            // Vector3 direction1 = (spawnPosition - transform.position).normalized;
            // direction1.y = 0; // Prevent rotation in the y-axis
            // Quaternion lookRotation1 = Quaternion.LookRotation(direction1);
            // transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation1, Time.deltaTime * 5f);

            // // Move towards their spawn position
            // navMeshAgent.destination = spawnPosition;
            // animator.SetBool("Iswalking", true); // Trigger moving animation

            animator.SetBool("Iswalking", true); // Stop moving animation when not chasing
            angle += originalSpeed * Time.deltaTime / radius;  // speed here is the distance per second along the circle's edge
            if (angle >= 360f) angle -= 360f;  // Keep the angle within 0-360 degrees

            // Calculate the new position around the circle
            Vector3 newPos = campManager.transform.position + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            // Move the NavMeshAgent to the new position
            navMeshAgent.destination = newPos;
        //   }
        //   else
        //   {
        //     animator.SetBool("Iswalking", false); // Stop moving animation when not chasing
        //   }
        }
      }
      else if (SceneManager.GetActiveScene().name == "BossLevel")
      {
        // Check if the player is within detection range
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
          // Move towards the player
          navMeshAgent.destination = player.position;
          animator.SetBool("Iswalking", true); // Trigger moving animation
        }
        else
        {
          animator.SetBool("Iswalking", false); // Stop moving animation when not chasing
        }
      }

      // Check if the player is within attack range
      if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
      {
        if (countA < 2)
        {
          animator.SetTrigger("StartAttack"); // Trigger sword attack
          countA++;
        }
        else
        {
          animator.SetTrigger("Throw"); // Trigger throw attack
          audioManager.PlaySFX(audioManager.explosionSFX);
          countA = 0; // Reset count
        }

        // Update the time of the last attack
        lastAttackTime = Time.time;
      }
    }
    // }
  }

  public void SwordAttack()
  {
    Debug.Log(player.GetComponent<BasePlayer>());
    player.GetComponent<BasePlayer>().TakeDamage(10);
  }

  public void ThrowAttack()
  {
    Debug.Log(player.GetComponent<BasePlayer>());
    player.GetComponent<BasePlayer>().TakeDamage(15);
  }



}
