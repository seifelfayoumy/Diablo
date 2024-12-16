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
    public float cooldownTimer = 0f;

    public bool delayCooldownStart = false;
    public float cooldownDelay = 0f;

    private float delayTimer = 0f;

    public Action<GameObject, Vector3?> UseAbilityAction;

    public void Use(GameObject enemy = null, Vector3? position = null)
    {
        if (isUnlocked && !isOnCooldown)
        {
            UseAbilityAction?.Invoke(enemy, position);

            if (delayCooldownStart)
            {
                isOnCooldown = true;
                cooldownTimer = cooldown;
                delayTimer = cooldownDelay;
            }
            else
            {
                isOnCooldown = true;
                cooldownTimer = cooldown;
            }
        }
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (isOnCooldown)
        {
            if (delayCooldownStart)
            {
                delayTimer -= deltaTime;
                if (delayTimer <= 0f)
                {
                    cooldownTimer = cooldown;
                    delayCooldownStart = false;
                }
            }
            else
            {
                cooldownTimer -= deltaTime;
                if (cooldownTimer <= 0f)
                {
                    isOnCooldown = false;
                    cooldownTimer = 0f;
                }
            }
        }
    }

    public float GetCooldownRemaining()
    {
        return cooldownTimer;
    }
}
