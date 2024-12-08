// AbilityManager.cs
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AbilityManager : MonoBehaviour
{
  public List<Ability> abilities; // List of all abilities for the player

  public BasePlayer player;
  public PlayerStats playerStats;

  private bool isSelectingTarget = true;
  private Ability currentAbility;
  private Vector3 selectedPosition;
  private GameObject selectedEnemy;

  void Start()
  {
    if (player == null)
      player = GetComponent<BasePlayer>();
    if (playerStats == null)
      playerStats = player.GetComponent<PlayerStats>();

    InitializeAbilities();
    currentAbility = abilities.Find(a => a.abilityType == AbilityType.Basic);
  }

  void Update()
  {
    // Update cooldowns
    foreach (var ability in abilities)
    {
      ability.UpdateCooldown(Time.deltaTime);
    }

    // Handle ability activation and target selection
    HandleInput();
  }

  void InitializeAbilities()
  {
    foreach (var ability in abilities)
    {
      // Assign the UseAbilityAction delegate based on abilityName
      switch (ability.abilityName)
      {
        // Barbarian Abilities
        case "Bash":
          ability.UseAbilityAction = (enemy, pos) => player.GetComponent<Barbarian>().Bash(enemy);
          break;
        case "Shield":
          ability.UseAbilityAction = (enemy, pos) => player.GetComponent<Barbarian>().Shield();
          break;
        case "IronMaelstorm":
          ability.UseAbilityAction = (enemy, pos) => player.GetComponent<Barbarian>().IronMaelstorm();
          break;
        case "Charge":
          ability.UseAbilityAction = (enemy, pos) => player.GetComponent<Barbarian>().Charge(pos.Value);
          break;

        // Sorcerer Abilities
        case "Fireball":
          ability.UseAbilityAction = (enemy, pos) => player.GetComponent<Sorcerer>().Fireball(enemy);
          break;
        case "Teleport":
          ability.UseAbilityAction = (enemy, pos) => player.GetComponent<Sorcerer>().Teleport(pos.Value);
          break;
        case "Clone":
          ability.UseAbilityAction = (enemy, pos) => player.GetComponent<Sorcerer>().Clone(pos.Value);
          break;
        case "Inferno":
          ability.UseAbilityAction = (enemy, pos) => player.GetComponent<Sorcerer>().Inferno(pos.Value);
          break;

        // Rogue Abilities
        case "Arrow":
          ability.UseAbilityAction = (enemy, pos) => player.GetComponent<Rogue>().Arrow(enemy);
          break;
        case "SmokeBomb":
          ability.UseAbilityAction = (enemy, pos) => player.GetComponent<Rogue>().SmokeBomb();
          break;
        case "Dash":
          ability.UseAbilityAction = (enemy, pos) => player.GetComponent<Rogue>().Dash(pos.Value);
          break;
        case "ShowerOfArrows":
          ability.UseAbilityAction = (enemy, pos) => player.GetComponent<Rogue>().ShowerOfArrows(pos.Value);
          break;

        default:
          Debug.LogWarning($"AbilityManager: Unknown ability {ability.abilityName}");
          break;
      }
    }
  }

  void HandleInput()
  {
    if (Input.GetKeyDown(KeyCode.W))
    {

      var ability = abilities.Find(a => a.abilityType == AbilityType.Defensive);
      var abilityName = ability.abilityName;
      UseAbility(abilityName);
    }
    if (Input.GetKeyDown(KeyCode.Q))
    {
      var ability = abilities.Find(a => a.abilityType == AbilityType.WildCard);
      var abilityName = ability.abilityName;
      UseAbility(abilityName);
    }
    if (Input.GetKeyDown(KeyCode.E))
    {
      var ability = abilities.Find(a => a.abilityType == AbilityType.Ultimate);
      var abilityName = ability.abilityName;
      UseAbility(abilityName);
    }
    // If currently selecting a target, wait for right-click
    if (isSelectingTarget && Input.GetMouseButtonDown(1))
    {
      RaycastHit hit;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out hit))
      {
        if (currentAbility.activationType == ActivationType.SelectEnemy)
        {
          GameObject enemy = hit.collider.gameObject;
          selectedEnemy = enemy;
          ExecuteAbility(currentAbility, enemy, null);
        }
        else if (currentAbility.activationType == ActivationType.SelectPosition)
        {
          Vector3 position = hit.point;
          selectedPosition = position;
          ExecuteAbility(currentAbility, null, position);
        }
      }
      isSelectingTarget = true;
      currentAbility = abilities.Find(a => a.abilityType == AbilityType.Basic);
    }
  }

  // Public method to use an ability by name
  public void UseAbility(string abilityName)
  {
    var ability = abilities.Find(a => a.abilityName == abilityName);
    if (ability != null)
    {
      if (ability.isUnlocked && !ability.isOnCooldown)
      {
        // Check activation type
        if (ability.activationType == ActivationType.Instant)
        {
          ability.Use();
        }
        else if (ability.activationType == ActivationType.SelectEnemy || ability.activationType == ActivationType.SelectPosition)
        {
          isSelectingTarget = true;
          currentAbility = ability;
          // Optional: Provide UI feedback to select target
        }
      }
    }
  }

  // Execute the ability after target selection
  void ExecuteAbility(Ability ability, GameObject enemy, Vector3? position)
  {
    ability.Use(enemy, position);
  }

  // Unlock a specific ability
  public void UnlockAbility(string abilityName)
  {
    var ability = abilities.Find(a => a.abilityName == abilityName);
    if (ability != null && !ability.isUnlocked && playerStats.abilityPoints > 0)
    {
      ability.isUnlocked = true;
      playerStats.abilityPoints--;
      // Update HUD
      FindObjectOfType<HUDManager>().UpdateHUD();
    }
  }

  // Get an ability by name
  public Ability GetAbility(string abilityName)
  {
    return abilities.Find(a => a.abilityName == abilityName);
  }

  // Accessor for PlayerStats
  public PlayerStats GetPlayerStats()
  {
    return playerStats;
  }
}
