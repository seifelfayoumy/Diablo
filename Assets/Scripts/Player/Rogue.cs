using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Rogue : BasePlayer
{
    public AudioClip dashSound;
    public AudioClip arrowSound;
    public AudioClip smokeBombSound;
    public AudioClip showerOfArrowsSound;
    public float arrowDamage = 5f;
    public float showerDamage = 10f;
    public float dashSpeed = 10f;
    public GameObject arrowPrefab;
    public GameObject smokeBombPrefab;
        public float smokeBombInitialDamage = 10f;
    public float smokeBombDamagePerSecond = 2f;
    public float smokeBombDuration = 5f;
    public GameObject showerArrowsPrefab;

    protected override void Start()
    {
        base.Start();
    }

    public void Arrow(GameObject enemy)
    {
        animator.SetTrigger("IsArrow");

        StartCoroutine(PerformArrow(enemy));
    }

    private IEnumerator PerformArrow(GameObject enemy)
    {
        yield return new WaitForSeconds(2f);

        Vector3 lookAt = new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z);
        transform.LookAt(lookAt);

        Vector3 spawnPosition = transform.position;
        spawnPosition.y = 1f;

        audioManager.PlaySFX(audioManager.arrowFiredSFX);
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, transform.rotation);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.Initialize(10, 5, enemy, playerStats);
        }

        if (enemy != null)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage((int)arrowDamage);
                if(enemyScript.GetHP() <= 0)
                    playerStats.GainXP(enemyScript.GetXP());
            }
        }
    }

    public void SmokeBomb(GameObject enemy = null, Vector3? position = null)
    {
        animator.SetTrigger("IsSmoke");

        GameObject inferno = Instantiate(smokeBombPrefab, transform.position, Quaternion.identity);
        SmokeBombBehavior infernoBehavior = inferno.GetComponent<SmokeBombBehavior>();
        if (infernoBehavior != null)
        {
            infernoBehavior.Initialize(this, smokeBombInitialDamage, smokeBombDamagePerSecond, smokeBombDuration);
        }

    }

    public void Dash(Vector3 position)
    {
        animator.SetBool("IsDash", true);
        audioManager.PlaySFX(audioManager.chargeDashSFX);

        StartCoroutine(PerformDash(position));
    }

    private IEnumerator PerformDash(Vector3 position)
    {
        float originalSpeed = navMeshAgent.speed;
        navMeshAgent.speed = 10f;

        Vector3 lookAt = new Vector3(position.x, transform.position.y, position.z);
        transform.LookAt(lookAt);
        navMeshAgent.SetDestination(position);


        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                break;
            }

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
            {
                break;
            }

            yield return null;
        }
        animator.SetBool("IsDash", false);
        navMeshAgent.speed = originalSpeed;
    }

    public void ShowerOfArrows(Vector3 position)
    {
        animator.SetTrigger("IsShower");

        audioManager.PlaySFX(audioManager.arrowFiredSFX);
        GameObject shower = Instantiate(showerArrowsPrefab, position, Quaternion.identity);
    }
}
