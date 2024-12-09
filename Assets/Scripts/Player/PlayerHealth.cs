// PlayerHealth.cs
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerStats stats;
    public bool isInvincible = false; // Added

    public void Initialize(PlayerStats playerStats)
    {
        stats = playerStats;
        stats.currentHP = stats.maxHP; // Initialize health to max HP
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        stats.currentHP -= damage;
        stats.currentHP = Mathf.Max(0, stats.currentHP); // Ensure health doesn't go below 0
       // Debug.Log(stats.currentHP);
    }

    public void Heal(int amount)
    {
        stats.currentHP += amount;
        stats.currentHP = Mathf.Min(stats.currentHP, stats.maxHP); // Ensure health doesn't exceed max HP
    }

    public bool IsDead => stats.currentHP <= 0; // Property to check if the player is dead

    // Method to set invincibility
    public void SetInvincibility(bool value)
    {
        isInvincible = value;
    }

    public bool IsInvincible => isInvincible;
}
