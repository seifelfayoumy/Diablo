using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithBehavior : Enemy
{
  public GameObject spikesPrefab;
  private Transform player;
  public GameObject minionPrefab;
  public GameObject shieldObject;
  public HealthBar shieldHealthbar;


  private enum BossPhase { Phase1, Phase2 }

  private BossPhase currentPhase = BossPhase.Phase1;
  private bool isAuraActive = false;
  private int maxShieldHP = 50;
  private int currentShieldHP = 50;

  public GameObject auraObject;

  public float spawnRadius = 15f;

  private bool isPerformingAction = false;
  private bool isTriggered = false;
  private List<GameObject> activeMinions = new List<GameObject>();

  public float actionCooldown = 10f;
  private int countAttack = 0;

    public GameObject gameWon;

    protected override void Start()
  {
    animator = GetComponent<Animator>();
    player = GameObject.FindGameObjectWithTag("Player")?.transform;
  }

  protected override void Update()
  {
    base.Update();
    if (player == null)
    {
      player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    if (player != null)
    {
      Vector3 direction = player.position - transform.position;
      direction.y = 0;
      Quaternion lookRotation = Quaternion.LookRotation(direction);
      transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    if (activeMinions.Count > 0)
    {
      isInvincible = true;
    }
    else
    {
      isInvincible = false;
    }

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
        animator.SetTrigger("Summon");
        audioManager.PlaySFX(audioManager.summonSFX);
        yield return new WaitForSeconds(1f);
        SummonMinions();
        countAttack++;
      }
      else if (countAttack == 1)
      {
        animator.SetTrigger("Divebomb");
        audioManager.PlaySFX(audioManager.bossStompsDownSFX);
        DiveBombAttack();
        yield return new WaitForSeconds(1f);
        countAttack = 0;
      }
    }
    else if (currentPhase == BossPhase.Phase2)
    {
      if (countAttack == 0)
      {
        animator.SetTrigger("IsSwinging");
        audioManager.PlaySFX(audioManager.bossSwingHandsSFX);
        doBloodSpikes();
        yield return new WaitForSeconds(1f);
        countAttack = 1;
      }
      else if (countAttack == 1 && isAuraActive == false)
      {
        animator.SetTrigger("IsCasting");
        audioManager.PlaySFX(audioManager.bossCastSpellSFX);
        doReflectiveAura();
        yield return new WaitForSeconds(1f);
        countAttack = 0;
      }
    }

    yield return new WaitForSeconds(actionCooldown);
    isPerformingAction = false;
  }

  private void TransitionToPhase2()
  {
    animator.SetTrigger("IsDead");
    audioManager.PlaySFX(audioManager.bossDiesSFX);
    StartCoroutine(Resurrect());
    StartCoroutine(StartPhase2());
  }

    private IEnumerator Resurrect()
    {
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("IsResurrect");
    }   
        private IEnumerator StartPhase2()
    {
        yield return new WaitForSeconds(5f);
    currentPhase = BossPhase.Phase2;
    currentHP = 50;
    currentShieldHP = maxShieldHP;
    shieldObject.SetActive(true);
    countAttack = 0;
  }   


  private void SummonMinions()
  {
    int minionCount = 3;
    List<Vector3> usedPositions = new List<Vector3>();

    for (int i = 0; i < minionCount; i++)
    {
      if (minionPrefab != null)
      {
        Vector3 randomPosition = Vector3.zero;
        bool isUniquePosition = false;

        while (!isUniquePosition)
        {
          randomPosition = GetRandomPositionAroundLilith(spawnRadius);
          isUniquePosition = true;

          foreach (Vector3 position in usedPositions)
          {
            if (Vector3.Distance(randomPosition, position) < 0.1f)
            {
              isUniquePosition = false;
              break;
            }
          }
        }

        usedPositions.Add(randomPosition);

        GameObject minion = Instantiate(minionPrefab, randomPosition, Quaternion.identity);
        activeMinions.Add(minion);

        MinionWatcher(minion);
      }
    }
  }

  private Vector3 GetRandomPositionAroundLilith(float radius)
  {
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
      yield return null;
    }

    if (activeMinions.Contains(minion))
    {
      Debug.Log("Minion destroyed and removed from activeMinions list.");
      activeMinions.Remove(minion);
    }
  }



  public void DiveBombAttack()
  {
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

    if (currentPhase == BossPhase.Phase1)
    {
      audioManager.PlaySFX(audioManager.bossHitSFX);
      currentHP -= damage;
      currentHP = Mathf.Max(0, currentHP);

      if (currentHP <= 0)
      {
        TransitionToPhase2();
      }
    }
    else if (currentPhase == BossPhase.Phase2)
    {
      if (isAuraActive)
      {
        player.GetComponent<BasePlayer>().TakeDamage(damage + 15);
        isAuraActive = false;
        return;
      }

      if (currentShieldHP > 0)
      {
        int shieldDamage = Mathf.Min(damage, currentShieldHP);

        currentShieldHP -= shieldDamage;

        int remainingDamage = damage - shieldDamage;
        if (remainingDamage > 0)
        {
          currentHP -= remainingDamage;
          currentHP = Mathf.Max(0, currentHP);
        }

        if (currentShieldHP <= 0)
        {
          currentShieldHP = 0;
          shieldObject.SetActive(false);

          StartCoroutine(RegenerateShield());

          player.GetComponent<BasePlayer>().TakeDamage(remainingDamage);
        }

        if (currentShieldHP > 0)
        {
          shieldHealthbar.SetHealth(currentShieldHP);
        }

        return;
      }
      else
      {
        audioManager.PlaySFX(audioManager.bossHitSFX);
        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);

        if (currentHP <= 0)
        {
          animator.SetTrigger("IsDead");
          audioManager.PlaySFX(audioManager.bossDiesSFX);
          Time.timeScale = 0f;
          gameWon.SetActive(true);
        }
      }
    }
  }

  private IEnumerator RegenerateShield()
  {
    yield return new WaitForSeconds(10f);

    currentShieldHP = maxShieldHP;
    shieldObject.SetActive(true);
    shieldHealthbar.SetHealth(currentShieldHP);
    isAuraActive = true;
    Debug.Log("Shield regenerated!");
  }


  public void doBloodSpikes()
  {
    GameObject spikes = Instantiate(spikesPrefab, new Vector3(-80f, 0.5f, -0.75f), transform.rotation);
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