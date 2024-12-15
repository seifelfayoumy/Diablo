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
    public AudioSource audioSource;

    // Animation Variables
    protected Animator animator;

        // Healing Potions and Rune Fragments
    // public int playerStats.healingPotions = 0;
    // public int playerStats.runeFragments = 0;

    // Reference to HUDManager for updating UI
    private HUDManager hudManager;


    // Sound Effects
    public AudioClip walkSound;
    public AudioClip deathSound;
    public AudioClip attackSound;
        private AbilityManager abilityManager;

        public bool isInvincible = false;

    // Start and Initialization
    protected virtual void Start()
    {
                abilityManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        // Initialize player stats (could be assigned from Inspector)
        if (playerStats != null)
        {
            playerStats.maxHP = 200; // Example default value, to be set dynamically
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
            if (isWalking && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(walkSound); // Play walk sound
            }
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
            audioSource.PlayOneShot(deathSound); // Play death sound
        }else{
            animator.SetTrigger("Reaction"); // Trigger hit animation
        }
    }

    public void Heal(int amount)
    {
        playerHealth.Heal(amount);
    }


    // Handle picking up items
   void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.CompareTag("HealingPotion"))
        {
            if (playerStats.healingPotions < 3)
            {
                playerStats.healingPotions++;
                hudManager.UpdateHUD();
                Destroy(other.gameObject);
                // Optionally, play a pickup sound
            }
        }
        else if (other.CompareTag("RuneFragment"))
        {
            // Assuming each Rune Fragment is unique per camp
            playerStats.runeFragments++;
            hudManager.UpdateHUD();
            Destroy(other.gameObject);
            // Optionally, play a pickup sound
        }
    } 

    

    public void UseHealingPotion()
{
    if (playerStats.healingPotions > 0 && playerStats.currentHP < playerStats.maxHP)
    {
        animator.SetTrigger("Heal"); // Trigger potion use animation
        int healAmount = playerStats.maxHP / 2;
        playerHealth.Heal(healAmount);
        playerStats.healingPotions--;
        hudManager.UpdateHUD();
    }
}
}
