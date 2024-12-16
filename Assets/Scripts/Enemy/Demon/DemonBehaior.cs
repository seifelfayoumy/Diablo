using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemonBehavior : Enemy
{
  public Transform player;
  public Transform playerClone;
  public float attackRange = 5f;
  public float detectionRange = 10f;
  public float moveSpeed = 3f;
  public float attackCooldown = 2f;
  private int countA = 0;
  private float lastAttackTime = 0f;
  public GameObject sword;

  public float radius = 5f;
  private float angle = 0f;


  public Vector3 spawnPosition;

  protected override void Start()
  {
    base.Start();
    animator = GetComponent<Animator>();
    spawnPosition = this.transform.position;
    attackCooldown = 10f;

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
      animator.SetBool("Iswalking", true);
      angle += originalSpeed * Time.deltaTime / radius;
      if (angle >= 360f) angle -= 360f;

      Vector3 newPos = campManager.transform.position + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);

      navMeshAgent.destination = newPos;

      return;
    }

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
      float distanceToPlayer = Vector3.Distance(transform.position, player.position);

      Vector3 direction = (player.position - transform.position).normalized;
      direction.y = 0;
      Quaternion lookRotation = Quaternion.LookRotation(direction);
      transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

      float distanceToPlayerFromCampfire = Vector3.Distance(campManager.runeFragmentSpawnPoint.position, player.position);
      float distanceToSpawnPosition = Vector3.Distance(transform.position, spawnPosition);

      if (distanceToPlayerFromCampfire <= detectionRange)
      {
        if(distanceToPlayer > attackRange)
        {
          navMeshAgent.destination = player.position;
          animator.SetBool("IsRunning", true);
        }
        else
        {
          animator.SetBool("IsRunning", false);
        }
      }
      else
      {
        animator.SetBool("IsRunning", false);
        animator.SetBool("Iswalking", true);
        angle += originalSpeed * Time.deltaTime / radius;
        if (angle >= 360f) angle -= 360f;

        Vector3 newPos = campManager.transform.position + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
        navMeshAgent.destination = newPos;
      }

      if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
      {
        if (countA < 2)
        {
          animator.SetTrigger("StartAttack");
          countA++;
        }
        else
        {
          animator.SetTrigger("Throw");
          audioManager.PlaySFX(audioManager.explosionSFX);
          countA = 0;
        }

        lastAttackTime = Time.time;
      }
    }
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
