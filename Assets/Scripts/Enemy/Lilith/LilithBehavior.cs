using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithBehavior : Enemy
{
  public GameObject spikesPrefab; // Reference to the player's transform
  private Transform player; // Reference to the player's transform
  public GameObject minionPrefab; // Reference to the minion prefab
  public GameObject shieldObject;
  public HealthBar shieldHealthbar;


  private enum BossPhase { Phase1, Phase2 }

  private BossPhase currentPhase = BossPhase.Phase1;
  private bool isAuraActive = false;
  private int maxShieldHP = 50;
  private int currentShieldHP = 50;

  public GameObject auraObject;

  public float spawnRadius = 15f; // Radius within which minions will be spawned

  private bool isPerformingAction = false; // To prevent overlapping actions
  private bool isTriggered = false; // To start actions only after the player triggers it
  private List<GameObject> activeMinions = new List<GameObject>(); // List to track active minions

  public float actionCooldown = 10f; // Cooldown between actions
  private int countAttack = 0; // Tracks the sequence of actions (0 = Summon, 1 = Divebomb)

  protected override void Start()
  {
    // base.Start();
    animator = GetComponent<Animator>();
    player = GameObject.FindGameObjectWithTag("Player")?.transform; // Find the player GameObject

  }

  protected override void Update()
  {
    base.Update();
    // Make Lilith look at the player if it exists
    if (player == null)
    {
      player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    if (player != null)
    {
      Vector3 direction = player.position - transform.position;
      direction.y = 0; // Keep the y-axis the same to prevent tilting up/down
      Quaternion lookRotation = Quaternion.LookRotation(direction);
      transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Adjust rotation speed as needed
    }
    if (activeMinions.Count > 0)
    {
      isInvincible = true;
      // Debug.Log("Lilith is invincible while minions are active.");
    }
    else
    {
      isInvincible = false;
    }



    // The attack sequence should only proceed if Lilith has been triggered and is not performing an action
    if (isTriggered && !isPerformingAction)
    {
      StartCoroutine(PerformNextAction());
    }

    if (shieldHealthbar != null)
    {
      shieldHealthbar.SetHealth(currentShieldHP);
      shieldHealthbar.SetHealthText(currentShieldHP);
      shieldHealthbar.SetMaxHealth(maxShieldHP);

    }

    if (auraObject != null)
    {
      auraObject.SetActive(isAuraActive);
    }

  }



  private IEnumerator PerformNextAction()
  {
    isPerformingAction = true;


    if (currentPhase == BossPhase.Phase1)
    {


      if (activeMinions.Count != 0)
      {
        countAttack = 1;
      }



      if (countAttack == 0 && activeMinions.Count == 0)
      {
        // Summon minions
        animator.SetTrigger("Summon");
        audioManager.PlaySFX(audioManager.summonSFX);
        yield return new WaitForSeconds(1f); // Wait for the summon animation
        SummonMinions();
        countAttack++;
      }
      else if (countAttack == 1)
      {
        // Perform Divebomb
        animator.SetTrigger("Divebomb");
        audioManager.PlaySFX(audioManager.bossStompsDownSFX);
        DiveBombAttack();
        yield return new WaitForSeconds(1f); // Wait for the divebomb animation
        countAttack = 0; // Reset to summon in the next cycle
      }
    }
    else if (currentPhase == BossPhase.Phase2)
    {
      if (countAttack == 0)
      {
        // Perform Blood Spikes
        animator.SetTrigger("IsSwinging");
        audioManager.PlaySFX(audioManager.bossSwingHandsSFX);
        doBloodSpikes();
        yield return new WaitForSeconds(1f); // Wait for the spikes animation
        countAttack = 1; // Reset to summon in the next cycle
      }
      else if (countAttack == 1 && isAuraActive == false)
      {
        // Perform Reflective Aura
        animator.SetTrigger("IsCasting");
        audioManager.PlaySFX(audioManager.bossCastSpellSFX);
        doReflectiveAura();
        yield return new WaitForSeconds(1f); // Wait for the aura animation
        countAttack = 0; // Reset to summon in the next cycle
      }
    }

    // Wait for cooldown
    yield return new WaitForSeconds(actionCooldown);
    isPerformingAction = false;
  }

  private void TransitionToPhase2()
  {


    animator.SetTrigger("IsDead"); // Trigger Phase 2 animation
    //set trigges IsResurrect after 2 seconds
    StartCoroutine(Resurrect());
    StartCoroutine(StartPhase2());
    // Optionally, trigger some Phase 2 visual or audio effects here


    // Additional Phase 2 logic can be placed here (e.g., spawning stronger minions)
  }

    private IEnumerator Resurrect()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        animator.SetTrigger("IsResurrect");
    }   
        private IEnumerator StartPhase2()
    {
        yield return new WaitForSeconds(5f); // Wait for 2 seconds
    currentPhase = BossPhase.Phase2;
    currentHP = 50; // Reset Phase 2 health
    currentShieldHP = maxShieldHP; // Reset shield HP
    shieldObject.SetActive(true); // Show the shield
    countAttack = 0; // Reset the attack sequence
  }   


  private void SummonMinions()
  {
    int minionCount = 3; // Adjust this to change how many minions to spawn each time
    List<Vector3> usedPositions = new List<Vector3>(); // Track used positions to avoid duplicates

    for (int i = 0; i < minionCount; i++)
    {
      if (minionPrefab != null)
      {
        Vector3 randomPosition = Vector3.zero; // Initialize to avoid the unassigned error
        bool isUniquePosition = false;

        // Keep generating a new position until we find one that hasn't been used
        while (!isUniquePosition)
        {
          randomPosition = GetRandomPositionAroundLilith(spawnRadius);
          isUniquePosition = true;

          // Check if the generated position is already in the list of used positions
          foreach (Vector3 position in usedPositions)
          {
            if (Vector3.Distance(randomPosition, position) < 0.1f) // Use a small tolerance to check for near duplicates
            {
              isUniquePosition = false;
              break;
            }
          }
        }

        // Add the new position to the list of used positions
        usedPositions.Add(randomPosition);

        // Instantiate the minion at the unique position
        GameObject minion = Instantiate(minionPrefab, randomPosition, Quaternion.identity);
        activeMinions.Add(minion);

        // Track when the minion is destroyed
        MinionWatcher(minion);
      }
    }
  }

  private Vector3 GetRandomPositionAroundLilith(float radius)
  {
    // Generate a random point within a radius around the Lilith's position
    Vector2 randomCircle = Random.insideUnitCircle * radius;
    Vector3 randomPosition = new Vector3(randomCircle.x, 0, randomCircle.y) + transform.position;
    return randomPosition;
  }

  private void MinionWatcher(GameObject minion)
  {
    StartCoroutine(WatchMinion(minion));
  }

  private IEnumerator WatchMinion(GameObject minion)
  {
    while (minion != null)
    {
      yield return null; // Wait until the minion is destroyed
    }

    if (activeMinions.Contains(minion))
    {
      Debug.Log("Minion destroyed and removed from activeMinions list.");
      activeMinions.Remove(minion);
    }
  }



  public void DiveBombAttack()
  {
    // Check if the player is within a radius of 10 units around Lilith
    if (player != null && Vector3.Distance(transform.position, player.position) <= 10f)
    {
      Debug.Log("Divebomb attack executed on player: " + player.GetComponent<BasePlayer>());
      player.GetComponent<BasePlayer>().TakeDamage(20);
    }
    else
    {
      Debug.Log("Player is not within the range for a divebomb attack.");
    }
  }

  public void doReflectiveAura()
  {
    shieldObject.SetActive(true);
    isAuraActive = true;
  }

  public override void TakeDamage(int damage)
  {
    if (isTriggered == false)
    {
      isTriggered = true;
      if (!isPerformingAction)
      {
        StartCoroutine(PerformNextAction());
      }
    }

    if (isInvincible)
    {
      return;
    }

    // Phase 1: Handle damage and transition to Phase 2 when health reaches zero
    if (currentPhase == BossPhase.Phase1)
    {
      currentHP -= damage;
      currentHP = Mathf.Max(0, currentHP);

      if (currentHP <= 0)
      {
        // Transition to Phase 2
        TransitionToPhase2();
      }
    }
    // Phase 2: Handle shield and main health damage
    else if (currentPhase == BossPhase.Phase2)
    {
      // If Reflective Aura is active, reflect damage back to the player
      if (isAuraActive)
      {
        player.GetComponent<BasePlayer>().TakeDamage(damage + 15); // Reflect damage to the player
        isAuraActive = false; // Deactivate the aura after it reflects damage
        return; // Stop further damage processing
      }

      // If shield is active, absorb damage with the shield
      if (currentShieldHP > 0)
      {
        // Calculate how much damage will go to the shield
        int shieldDamage = Mathf.Min(damage, currentShieldHP);

        // Subtract from the shield first
        currentShieldHP -= shieldDamage;

        // Apply the remaining damage to Lilith's health (if any)
        int remainingDamage = damage - shieldDamage;
        if (remainingDamage > 0)
        {
          currentHP -= remainingDamage;
          currentHP = Mathf.Max(0, currentHP);
        }

        // If the shield is completely destroyed, deactivate it
        if (currentShieldHP <= 0)
        {
          currentShieldHP = 0;
          shieldObject.SetActive(false); // Deactivate the shield visual

          // Start shield regeneration after 10 seconds
          StartCoroutine(RegenerateShield());

          // Inflict damage on the player because the shield broke
          player.GetComponent<BasePlayer>().TakeDamage(remainingDamage); // Apply full remaining damage to player
        }

        // Update shield health bar if the shield is still active
        if (currentShieldHP > 0)
        {
          shieldHealthbar.SetHealth(currentShieldHP);
        }

        return; // Stop further damage processing (shield absorbed it)
      }
      else
      {
        // No shield, damage Lilith's health directly
        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);

        if (currentHP <= 0)
        {
          animator.SetTrigger("IsDead");
            audioManager.PlaySFX(audioManager.bossDiesSFX);
        }
      }
    }
  }

  // Regenerate the shield after 10 seconds of being destroyed
  private IEnumerator RegenerateShield()
  {
    yield return new WaitForSeconds(10f); // Wait for 10 seconds

    // Regenerate shield health and activate the shield
    currentShieldHP = maxShieldHP;
    shieldObject.SetActive(true); // Reactivate the shield object
    shieldHealthbar.SetHealth(currentShieldHP); // Update the shield health bar
    isAuraActive = true; // Reactivate the aura
    Debug.Log("Shield regenerated!");
  }


  public void doBloodSpikes()
  {
    //new Vector3(0.2f, 0.5f,-1.4f)
    GameObject spikes = Instantiate(spikesPrefab, new Vector3(7f, 0.5f, -0.3f), transform.rotation);
    spikes.SetActive(true);
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, 12f);
    foreach (var hit in hitColliders)
    {
      if (hit.CompareTag("Player"))
      {
        BasePlayer player = hit.GetComponent<BasePlayer>();
        if (player != null)
        {
          player.TakeDamage(30);
        }
      }
    }
  }
}