// CheatManager.cs
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    private BasePlayer player;
    private PlayerStats playerStats;
    private AbilityManager abilityManager;

    void Start()
    {
        player = FindObjectOfType<BasePlayer>();
        playerStats = player.GetComponent<PlayerStats>();
        abilityManager = FindObjectOfType<AbilityManager>();
    }

    void Update()
    {
        // Heal: Press "H"
        if (Input.GetKeyDown(KeyCode.H))
        {
            player.Heal(20);
            FindObjectOfType<HUDManager>().UpdateHUD();
            Debug.Log("Cheat: Healed 20 HP");
        }

        // Decrement Health: Press "D"
        if (Input.GetKeyDown(KeyCode.D))
        {
            player.TakeDamage(20);
            FindObjectOfType<HUDManager>().UpdateHUD();
            Debug.Log("Cheat: Took 20 Damage");
        }

        // Toggle Invincibility: Press "I"
        if (Input.GetKeyDown(KeyCode.I))
        {
            player.playerHealth.SetInvincibility(!player.playerHealth.isInvincible);
            Debug.Log($"Cheat: Invincibility {(player.playerHealth.isInvincible ? "Enabled" : "Disabled")}");
        }

        // Toggle Slow Motion: Press "M"
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = (Time.timeScale == 1f) ? 0.5f : 1f;
            Debug.Log($"Cheat: Slow Motion {(Time.timeScale == 0.5f ? "Enabled" : "Disabled")}");
        }

        // Toggle Cool Down: Press "C"
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (var ability in abilityManager.abilities)
            {
                ability.isOnCooldown = !ability.isOnCooldown;
                ability.cooldownTimer = (ability.isOnCooldown) ? ability.cooldown : 0f;
            }
            Debug.Log("Cheat: Toggle Cooldowns");
        }

        // Unlock Abilities: Press "U"
        if (Input.GetKeyDown(KeyCode.U))
        {
            foreach (var ability in abilityManager.abilities)
            {
                ability.isUnlocked = true;
            }
            FindObjectOfType<HUDManager>().UpdateHUD();
            Debug.Log("Cheat: All Abilities Unlocked");
        }

        // Gain Ability Points: Press "A"
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerStats.abilityPoints += 1;
            FindObjectOfType<HUDManager>().UpdateHUD();
            Debug.Log("Cheat: Gained 1 Ability Point");
        }

        // Gain XP: Press "X"
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerStats.GainXP(100);
            FindObjectOfType<HUDManager>().UpdateHUD();
            Debug.Log("Cheat: Gained 100 XP");
        }
    }
}