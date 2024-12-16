using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BasePlayer : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    protected NavMeshAgent navMeshAgent;
    
    public PlayerStats playerStats;
    public PlayerHealth playerHealth;

    protected Animator animator;

    private HUDManager hudManager;

    private AbilityManager abilityManager;

    public bool isInvincible = false;

    public AudioManager audioManager;

    public GameObject gameOverPanel;

    protected virtual void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        abilityManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    if (playerStats != null && SceneManager.GetActiveScene().name == "MainLevel")
    {
      playerStats.maxHP = 100;
      playerStats.currentHP = playerStats.maxHP;
    }
    else if (playerStats != null && SceneManager.GetActiveScene().name == "BossLevel")
    {
      playerStats.maxHP = 400;
      playerStats.currentHP = playerStats.maxHP;
      foreach (var ability in abilityManager.abilities)
      {
        ability.isUnlocked = true;
      }
      playerStats.level = 4;
      playerStats.runeFragments = 3;
    }

        if (playerHealth != null)
        {
            playerHealth.Initialize(playerStats);
        }
         hudManager = FindObjectOfType<HUDManager>();
    }

    protected virtual void Update()
    {
        HandleMovement();

        if(Input.GetKeyDown(KeyCode.F))
        {
            UseHealingPotion();
        }

    }

    private void HandleMovement()
    {
        if (navMeshAgent.enabled && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) )
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    return;
                }
                Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                transform.LookAt(lookAt);

                navMeshAgent.destination = hit.point;
            }
        }
            bool isWalking = navMeshAgent.velocity != Vector3.zero;
            animator.SetBool("IsWalking", isWalking);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            Debug.Log("Player is invincible!");
            return;
        }
        
        playerHealth.TakeDamage(damage);
        if (playerHealth.IsDead)
        {
            animator.SetTrigger("IsDead");
            audioManager.PlaySFX(audioManager.wandererDiesSFX);

        }else{
            animator.SetTrigger("Reaction");
            audioManager.PlaySFX(audioManager.wandererDamageSFX);
        }
    }

    public void Heal(int amount)
    {
        playerHealth.Heal(amount);
    }

   void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealingPotion"))
        {
            if (playerStats.healingPotions < 3)
            {
                playerStats.healingPotions++;
                hudManager.UpdateHUD();
                audioManager.PlaySFX(audioManager.pickupSFX);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("RuneFragment"))
        {
            playerStats.runeFragments++;
            hudManager.UpdateHUD();
            audioManager.PlaySFX(audioManager.pickupSFX);
            Destroy(other.gameObject);
        }
    } 

    public void UseHealingPotion()
    {
        if (playerStats.healingPotions > 0 && playerStats.currentHP < playerStats.maxHP)
        {
            animator.SetTrigger("Heal");
            audioManager.PlaySFX(audioManager.healingPotionSFX);
            int healAmount = playerStats.maxHP / 2;
            playerHealth.Heal(healAmount);
            playerStats.healingPotions--;
            hudManager.UpdateHUD();
        }
    }
    public void GameOver()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(true);
        Debug.Log("Game Over!");
    }
}
