using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;  // Needed for checking UI interaction
using UnityEngine.SceneManagement;

public class BasePlayer : MonoBehaviour
{
    // Movement Variables
    public float moveSpeed = 3.5f;
    protected NavMeshAgent navMeshAgent;
    
    // Health Variables
    public PlayerStats playerStats; // Reference to stats for health management
    public PlayerHealth playerHealth; // Reference to health management

    // Animation Variables
    protected Animator animator;

    // Healing Potions and Rune Fragments
    // public int playerStats.healingPotions = 0;
    // public int playerStats.runeFragments = 0;

    // Reference to HUDManager for updating UI
    private HUDManager hudManager;

    private AbilityManager abilityManager;

    public bool isInvincible = false;

    public AudioManager audioManager;

    //game over panal
    public GameObject gameOverPanel; // Assign the GameOverPanel in the Inspector

    // Start and Initialization
    protected virtual void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        abilityManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        // Initialize player stats (could be assigned from Inspector)
        if (playerStats != null)
        {
            playerStats.maxHP = 100; // Example default value, to be set dynamically
            playerStats.currentHP = playerStats.maxHP;
        }

        if (playerHealth != null)
        {
            playerHealth.Initialize(playerStats); // Initialize with stats
        }
         hudManager = FindObjectOfType<HUDManager>();
    }

    // Update function for movement
    protected virtual void Update()
    {
        HandleMovement();

        if(Input.GetKeyDown(KeyCode.F))
        {
            UseHealingPotion();
        }

    }

    // Handle movement logic (click-to-move)
    private void HandleMovement()
    {
        if (navMeshAgent.enabled && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return; // Do nothing if clicking on UI
            }
            
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) )
            {
                // If the click is on the UI layer, don't do anything
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

    // Common health management
    public void TakeDamage(int damage)
    {
        if(playerHealth.IsDead)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads the current scene
            return;
        }

        if (isInvincible)
        {
            Debug.Log("Player is invincible!");
            return;
        }
        
        playerHealth.TakeDamage(damage);
        if (playerHealth.IsDead)
        {
            animator.SetTrigger("IsDead"); // Trigger death animation
            audioManager.PlaySFX(audioManager.wandererDiesSFX);

        }else{
            animator.SetTrigger("Reaction"); // Trigger hit animation
            audioManager.PlaySFX(audioManager.wandererDamageSFX);
        }
    }

    public void Heal(int amount)
    {
        playerHealth.Heal(amount);
    }


    // Handle picking up items
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
        Time.timeScale = 1f; // Ensure time scale is normal
        gameOverPanel.SetActive(true); // Show Game Over Panel
        Debug.Log("Game Over!");
    }
}
