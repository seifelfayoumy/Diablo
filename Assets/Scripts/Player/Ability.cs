using UnityEngine;
using System;

public enum AbilityType { Basic, Defensive, WildCard, Ultimate }
public enum ActivationType { Instant, SelectPosition, SelectEnemy }

public enum RangeType { Small, Medium, Large }

[System.Serializable]
public class Ability
{
    public string abilityName;
    public AbilityType abilityType;
    public ActivationType activationType;

    public RangeType rangeType;
    public float damage;
    public float cooldown;
    public bool isUnlocked = false;
    public bool isOnCooldown = false;
    public float cooldownTimer = 0f; // Changed to public

    public bool delayCooldownStart = false; // New flag to specify delayed cooldown start
    public float cooldownDelay = 0f; // Time to delay cooldown start

    private float delayTimer = 0f; // Timer to track the cooldown delay

    public Action<GameObject, Vector3?> UseAbilityAction; // Delegate for ability behavior

    // Method to use the ability
    public void Use(GameObject enemy = null, Vector3? position = null)
    {
        if (isUnlocked && !isOnCooldown)
        {
            // Execute ability action
            UseAbilityAction?.Invoke(enemy, position);

            // If cooldown is delayed, start the delay timer
            if (delayCooldownStart)
            {
                isOnCooldown = true;
                cooldownTimer = cooldown;
                delayTimer = cooldownDelay;  // Set delay timer instead of cooldown immediately
            }
            else
            {
                // No delay, start the cooldown immediately
                isOnCooldown = true;
                cooldownTimer = cooldown;
            }
        }
    }

    // Update cooldown timer
    public void UpdateCooldown(float deltaTime)
    {
        if (isOnCooldown)
        {
            if (delayCooldownStart)
            {
                // Handle the cooldown delay
                delayTimer -= deltaTime;
                if (delayTimer <= 0f)
                {
                    // Once the delay is over, start the actual cooldown timer
                    cooldownTimer = cooldown;
                    delayCooldownStart = false; // Reset the delay flag
                }
            }
            else
            {
                // Regular cooldown countdown
                cooldownTimer -= deltaTime;
                if (cooldownTimer <= 0f)
                {
                    isOnCooldown = false;
                    cooldownTimer = 0f;
                }
            }
        }
    }

    // Get remaining cooldown
    public float GetCooldownRemaining()
    {
        return cooldownTimer;
    }
}
