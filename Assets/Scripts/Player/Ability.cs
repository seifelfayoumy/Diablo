// Ability.cs
using UnityEngine;
using System;

public enum AbilityType { Basic, Defensive, WildCard, Ultimate }
public enum ActivationType { Instant, SelectPosition, SelectEnemy }

[System.Serializable]
public class Ability
{
    public string abilityName;
    public AbilityType abilityType;
    public ActivationType activationType;
    public float damage;
    public float cooldown;
    public bool isUnlocked = false;
    public bool isOnCooldown = false;
    public float cooldownTimer = 0f; // Changed to public

    public Action<GameObject, Vector3?> UseAbilityAction; // Delegate for ability behavior

    // Method to use the ability
    public void Use(GameObject enemy = null, Vector3? position = null)
    {
        if (isUnlocked && !isOnCooldown)
        {
            UseAbilityAction?.Invoke(enemy, position);
            isOnCooldown = true;
            cooldownTimer = cooldown;
        }
    }

    // Update cooldown timer
    public void UpdateCooldown(float deltaTime)
    {
        if (isOnCooldown)
        {
            cooldownTimer -= deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }

    // Get remaining cooldown
    public float GetCooldownRemaining()
    {
        return cooldownTimer;
    }
}
