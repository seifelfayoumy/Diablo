using UnityEngine;
using UnityEngine.AI;

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


    // Sound Effects
    public AudioClip walkSound;
    public AudioClip deathSound;
    public AudioClip attackSound;

    // Start and Initialization
    protected virtual void Start()
    {
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
    }

    // Update function for movement
    protected virtual void Update()
    {
        HandleMovement();

    }

    // Handle movement logic (click-to-move)
    private void HandleMovement()
    {
        if (navMeshAgent.enabled && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) )
            {
                //adjust rotation
                Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                transform.LookAt(lookAt);
                
                navMeshAgent.destination = hit.point;
            }
            // Vector3 destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // destination.y = transform.position.y; // Keep y-axis level constant
            // Debug.Log("Destination: " + destination);
            // navMeshAgent.SetDestination(destination);


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
        playerHealth.TakeDamage(damage);
        if (playerHealth.IsDead)
        {
            animator.SetBool("IsDead", true); // Trigger death animation
            audioSource.PlayOneShot(deathSound); // Play death sound
        }
    }

    public void Heal(int amount)
    {
        playerHealth.Heal(amount);
    }


}
