using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void ActivateBasicAbility()
    {
        // Basic Ability: Damage the enemy or do a simple action
        Debug.Log("Basic Ability Activated");
    }

    public void ActivateDefensiveAbility()
    {
        // Defensive Ability: Shield or similar defensive mechanic
        Debug.Log("Defensive Ability Activated");
    }

    public void ActivateWildCardAbility()
    {
        // Wild Card Ability: Special Ability, e.g., Teleport or Dash
        Debug.Log("Wild Card Ability Activated");
    }

    public void ActivateUltimateAbility()
    {
        // Ultimate Ability: Strongest attack or power
        Debug.Log("Ultimate Ability Activated");
    }
}
