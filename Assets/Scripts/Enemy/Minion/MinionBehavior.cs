using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinionBehavior : Enemy
{
    public Transform player;
    public Transform playerClone;
    public float attackRange = 5f;
    public float detectionRange = 10f;
    public float moveSpeed = 3f;
    public float attackCooldown = 5f;
    private int countA = 0;
    private float lastAttackTime = 0f;
    public GameObject sword;

    public Vector3 spawnPosition;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        spawnPosition = this.transform.position;
        attackCooldown = 7f;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected override void  Update()
    {
        base.Update();
        moveSpeed = originalSpeed;
        
        if(isStunned)
        {
            return;
        }

        if(base.currentHP <=0){
            return;        
        }

        if(!isAlerted && SceneManager.GetActiveScene().name == "MainLevel")
        {
            return;
        }

        playerClone = GameObject.FindGameObjectWithTag("PlayerClone")?.transform;
        if(playerClone != null)
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



            if(SceneManager.GetActiveScene().name == "MainLevel")
            {
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
                    if(distanceToSpawnPosition > 1)
                    {
                        Vector3 direction1 = (spawnPosition - transform.position).normalized;
                        direction1.y = 0;
                        Quaternion lookRotation1 = Quaternion.LookRotation(direction1);
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation1, Time.deltaTime * 5f);

                        navMeshAgent.destination = spawnPosition;
                        animator.SetBool("IsRunning", false);
                        animator.SetBool("Iswalking", true);
                    }
                    else
                    {
                        animator.SetBool("Iswalking", false);
                    }
                }
            }
            else if(SceneManager.GetActiveScene().name == "BossLevel")
            {
                if (distanceToPlayer > attackRange)
                {
                    navMeshAgent.destination = player.position;
                    animator.SetBool("IsRunning", true);
                }
                else
                {
                    animator.SetBool("IsRunning", false);
                }
            }

            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("Punch");
                SwordAttack();

                lastAttackTime = Time.time;
            }
        }
    }

    public void SwordAttack()
    {
        Debug.Log(player.GetComponent<BasePlayer>());
        player.GetComponent<BasePlayer>().TakeDamage(5);
    }

    public void ThrowAttack()
    {
        Debug.Log(player.GetComponent<BasePlayer>());
        player.GetComponent<BasePlayer>().TakeDamage(15);
    }
}
