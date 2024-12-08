using UnityEngine;

public class LilithPhase2Controller : MonoBehaviour
{
    private Animator animator;

    public GameObject shieldPrefab; // Reference to the shield prefab
    private GameObject activeShield; // To track the active shield instance

    public GameObject player; // Reference to the Player GameObject
    private PlayerStats playerStats; // Reference to the PlayerStats script

    private float health = 50f; // Phase 2 health
    private float shieldHealth = 50f;
    private bool shieldActive = true;

    private float shieldCooldown = 10f; // Shield regenerates after 10 seconds
    private float shieldTimer = 0f;

    private bool isReflectiveAuraActive = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats script not found on the Player GameObject!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not assigned in the Inspector!");
        }

        // Initialize Lilith's position in the arena
        transform.position = new Vector3(0, 0, 0); // Adjust this to the center of your arena
    }

    void Update()
    {
        if (health <= 0)
        {
            PlayDying();
        }
        else
        {
            HandleShieldRegeneration();
            AutomaticAttackPattern();
            LookAtPlayer();
        }
    }

    private void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0; // Prevent rotation on the Y-axis
            Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = rotationToPlayer;
        }
    }

    private void HandleShieldRegeneration()
    {
        if (!shieldActive)
        {
            shieldTimer += Time.deltaTime;
            if (shieldTimer >= shieldCooldown)
            {
                shieldActive = true;
                shieldHealth = 50f;
                shieldTimer = 0f;
                PlayShieldReform(); // Reform the shield
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (shieldActive)
        {
            shieldHealth -= damage;
            if (shieldHealth <= 0)
            {
                shieldActive = false;
                shieldHealth = 0;
                PlayShieldBreak();
            }
        }
        else if (isReflectiveAuraActive && playerStats != null)
        {
            playerStats.currentHP -= Mathf.RoundToInt(damage); // Directly reduce the player's currentHP
            Debug.Log($"Player took {damage} damage from Reflective Aura!");

            if (playerStats.currentHP <= 0)
            {
                Debug.Log("Player has died!");
                playerStats.currentHP = 0; // Prevent negative HP
            }
        }
        else
        {
            health -= damage;
            PlayGettingDamaged();
        }
    }

    private void AutomaticAttackPattern()
    {
        if (isReflectiveAuraActive)
        {
            PlayReflectiveAura();
        }
        else
        {
            PlayBloodSpikes();
        }
    }

    private void PlayIdle()
    {
        animator.Play("Idle");
    }

    private void PlaySummoning()
    {
        animator.Play("Summoning");
    }

    private void PlayBloodSpikes()
    {
        animator.Play("Divebomb");

        if (playerStats != null)
        {
            playerStats.currentHP -= 30; // Deal 30 damage to the player
            Debug.Log("Player took 30 damage from Blood Spikes!");

            if (playerStats.currentHP <= 0)
            {
                Debug.Log("Player has died!");
                playerStats.currentHP = 0; // Prevent negative HP
            }
        }
    }

    private void PlayReflectiveAura()
    {
        animator.Play("Cast Spell");

        if (playerStats != null)
        {
            playerStats.currentHP -= 15; // Reflective Aura deals 15 damage
            Debug.Log("Player took 15 damage from Reflective Aura!");

            if (playerStats.currentHP <= 0)
            {
                Debug.Log("Player has died!");
                playerStats.currentHP = 0; // Prevent negative HP
            }
        }

        isReflectiveAuraActive = false;
    }

    private void PlaySwingingHands()
    {
        animator.Play("Swinging Hands");
    }

    private void PlayGettingDamaged()
    {
        animator.Play("Getting Damaged Reaction");
    }

    private void PlayShieldReform()
    {
        if (activeShield == null) // Ensure no duplicate shields
        {
            activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            activeShield.transform.SetParent(transform); // Attach the shield to Lilith
        }

        animator.Play("Reformation/Resurrected"); // Play reformation animation
    }

    private void PlayShieldBreak()
    {
        if (activeShield != null)
        {
            Destroy(activeShield); // Remove the shield prefab
            activeShield = null;   // Clear the reference
        }

        animator.Play("Getting Damaged Reaction"); // Play a fallback animation
    }

    private void PlayStunned()
    {
        animator.Play("Stunned Reaction");
    }

    private void PlayDying()
    {
        if (activeShield != null)
        {
            Destroy(activeShield); // Clean up the shield on death
        }

        animator.Play("Dying");
        // Handle boss defeat logic
    }
}