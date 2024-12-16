using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerStats stats;
    public bool isInvincible = false;

    public void Initialize(PlayerStats playerStats)
    {
        stats = playerStats;
        stats.currentHP = stats.maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        stats.currentHP -= damage;
        stats.currentHP = Mathf.Max(0, stats.currentHP);
    }

    public void Heal(int amount)
    {
        stats.currentHP += amount;
        stats.currentHP = Mathf.Min(stats.currentHP, stats.maxHP);
    }

    public bool IsDead => stats.currentHP <= 0;

    public void SetInvincibility(bool value)
    {
        isInvincible = value;
    }

    public bool IsInvincible => isInvincible;
}
