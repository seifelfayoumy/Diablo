using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Barbarian : BasePlayer
{
  // Ability-specific variables
  public AudioClip shieldSound;
  public AudioClip bashSound;
  public AudioClip chargeSound;
  public AudioClip ironMaelstormSound;
  public float bashDamage = 5f;
  public float ironMaelstormDamage = 10f;
  public GameObject shieldObject;

  protected override void Start()
  {
    base.Start();
  }

  // Bash ability targeting an enemy
  public void Bash(GameObject enemy)
  {
    animator.SetTrigger("Bash");
    audioSource.PlayOneShot(bashSound);

    if (enemy != null)
    {
      Enemy enemyScript = enemy.GetComponent<Enemy>();
      if (enemyScript != null)
      {
        // Adjust rotation
        Vector3 position = enemy.transform.position;
        Vector3 lookAt = new Vector3(position.x, transform.position.y, position.z);
        transform.LookAt(lookAt);

        float distanceToEnemy = Vector3.Distance(transform.position, position);
        if (distanceToEnemy <= 5)
        {
          enemyScript.TakeDamage((int)bashDamage);
          if(enemyScript.GetHP() <= 0)
            playerStats.GainXP(enemyScript.GetXP());
        }
      }
    }
  }

  // Shield ability
  public void Shield(GameObject enemy = null, Vector3? position = null)
  {
    animator.SetTrigger("Shield");
    audioSource.PlayOneShot(shieldSound);

    // Spawn and activate shield
    GameObject shield = Instantiate(shieldObject, transform.position, Quaternion.identity);
    shield.transform.SetParent(transform);
    Destroy(shield, 3f);
  }

  // Iron Maelstorm ability
  public void IronMaelstorm(GameObject enemy = null, Vector3? position = null)
  {
    animator.SetTrigger("IronMaelstorm");
    audioSource.PlayOneShot(ironMaelstormSound);

    // Area damage around the player
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f); // Example range
    foreach (var hit in hitColliders)
    {
      if (hit.CompareTag("Enemy"))
      {
        Enemy enemyScript = hit.GetComponent<Enemy>();
        if (enemyScript != null)
        {
          enemyScript.TakeDamage((int)ironMaelstormDamage);
          if(enemyScript.GetHP() <= 0)
            playerStats.GainXP(enemyScript.GetXP());
        }
      }
    }
  }

  // Charge ability towards a position
  public void Charge(Vector3 position)
  {
    animator.SetBool("Charge", true);
    Debug.Log("Charging");
    audioSource.PlayOneShot(chargeSound);

    // Implement charging logic
    StartCoroutine(PerformCharge(position));
  }

private IEnumerator PerformCharge(Vector3 position)
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Charge") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
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
    animator.SetBool("Charge", false);
    navMeshAgent.speed = originalSpeed;
}



}
