using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Barbarian : BasePlayer
{
  public float bashDamage = 5f;
  public float ironMaelstormDamage = 10f;
  public GameObject shieldObject;

  private bool isCharge = false;

  protected override void Start()
  {
    base.Start();
  }

  public void Bash(GameObject enemy)
  {

    if (enemy != null)
    {
      Enemy enemyScript = enemy.GetComponent<Enemy>();
      if (enemyScript != null)
      {
        Vector3 position = enemy.transform.position;
        Vector3 lookAt = new Vector3(position.x, transform.position.y, position.z);
        transform.LookAt(lookAt);

        float distanceToEnemy = Vector3.Distance(transform.position, position);
        if (distanceToEnemy <= 5)
        {
          animator.SetTrigger("Bash");

          enemyScript.TakeDamage((int)bashDamage);
          if (enemyScript.GetHP() <= 0)
            playerStats.GainXP(enemyScript.GetXP());
        }
      }
    }
  }

  public void Shield(GameObject enemy = null, Vector3? position = null)
  {
    animator.SetTrigger("Shield");
    audioManager.PlaySFX(audioManager.shieldActivateSFX);

    GameObject shield = Instantiate(shieldObject, transform.position, Quaternion.identity);
    shield.transform.SetParent(transform);
    Destroy(shield, 3f);

    isInvincible = true;
    StartCoroutine(ShieldDuration());
  }


  private IEnumerator ShieldDuration()
  {
    yield return new WaitForSeconds(3f);
    isInvincible = false;
  }

  public void IronMaelstorm(GameObject enemy = null, Vector3? position = null)
  {
    animator.SetTrigger("IronMaelstorm");

    Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
    foreach (var hit in hitColliders)
    {
      if (hit.CompareTag("Enemy"))
      {
        Enemy enemyScript = hit.GetComponent<Enemy>();
        if (enemyScript != null)
        {
          enemyScript.TakeDamage((int)ironMaelstormDamage);
          if (enemyScript.GetHP() <= 0)
            playerStats.GainXP(enemyScript.GetXP());
        }
      }
    }
  }

  public void Charge(Vector3 position)
  {
    animator.SetBool("Charge", true);
    audioManager.PlaySFX(audioManager.chargeDashSFX);
    isCharge = true;
    StartCoroutine(PerformCharge(position));
  }

  private IEnumerator PerformCharge(Vector3 position)
  {
    float originalSpeed = navMeshAgent.speed;
    navMeshAgent.speed = 10f;

    Vector3 lookAt = new Vector3(position.x, transform.position.y, position.z);
    transform.LookAt(lookAt);
    navMeshAgent.SetDestination(position);

    bool hitBoss = false;
    while (true)
    {

      Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f);
      foreach (var hit in hitColliders)
      {
          if (hit.gameObject.CompareTag("Enemy"))
          {
            Enemy enemyScript = hit.gameObject.GetComponent<Enemy>();
            if (enemyScript != null)
            {
              if (hit.gameObject.name == "Lilith" && !hitBoss)
              {
                            hitBoss= true;
                            Debug.Log("Lilith hit charge");
                enemyScript.TakeDamage(20);
              }
              else if (hit.gameObject.name != "Lilith")
                        {
                playerStats.GainXP(enemyScript.GetXP());
                enemyScript.Die();
                            Debug.Log("Lilith ISNT hit charge");
                        }
            }
          }
      }
      if (animator.GetCurrentAnimatorStateInfo(0).IsName("Charge") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
      {
        break;
      }

      if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
      {
        break;
      }

      yield return null;
    }

    isCharge = false;

    animator.SetBool("Charge", false);
    navMeshAgent.speed = originalSpeed;
  }
}
