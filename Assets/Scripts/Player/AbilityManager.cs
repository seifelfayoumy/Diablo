using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AbilityManager : MonoBehaviour
{
  public List<Ability> abilities;

  public BasePlayer player;
  public PlayerStats playerStats;

  private bool isSelectingTarget = true;
  private Ability currentAbility;
  private Vector3 selectedPosition;
  private GameObject selectedEnemy;

  private bool defensiveW;
  private bool wildCardQ;
  private bool ultimateE;

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
    foreach (var ability in abilities)
    {
      ability.UpdateCooldown(Time.deltaTime);
    }

    HandleInput();
  }

  void InitializeAbilities()
  {
    foreach (var ability in abilities)
    {
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
    if(!defensiveW && !wildCardQ && !ultimateE)
    {
      if (Input.GetKeyDown(KeyCode.W))
      {
        var ability = abilities.Find(a => a.abilityType == AbilityType.Defensive);
        var abilityName = ability.abilityName;
        defensiveW = true;
        UseAbility(abilityName);
      }
      if (Input.GetKeyDown(KeyCode.Q))
      {
        var ability = abilities.Find(a => a.abilityType == AbilityType.WildCard);
        var abilityName = ability.abilityName;
        wildCardQ = true;
        UseAbility(abilityName);
      }
      if (Input.GetKeyDown(KeyCode.E))
      {
        var ability = abilities.Find(a => a.abilityType == AbilityType.Ultimate);
        var abilityName = ability.abilityName;
        ultimateE = true;
        UseAbility(abilityName);
      }
    }

    if (isSelectingTarget && Input.GetMouseButtonDown(1))
    {
      RaycastHit hit;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out hit))
      {
        if (currentAbility.activationType == ActivationType.SelectEnemy)
        {
          GameObject enemy = hit.collider.gameObject;
          if(enemy.CompareTag("Enemy"))
          {
            Vector3 position = enemy.transform.position;
            Vector3 lookAt = new Vector3(position.x, transform.position.y, position.z);
            transform.LookAt(lookAt);

            float distanceToEnemy = Vector3.Distance(transform.position, position);
            if(currentAbility.rangeType == RangeType.Small && distanceToEnemy > 5){
              return;
            }
            if(currentAbility.rangeType == RangeType.Medium && distanceToEnemy > 10){
              return;
            }
            selectedEnemy = enemy;
            ExecuteAbility(currentAbility, enemy, null);
          }
        }
        else if (currentAbility.activationType == ActivationType.SelectPosition)
        {
          
          Vector3 position = hit.point;
          selectedPosition = position;

          Vector3 lookAt = new Vector3(position.x, transform.position.y, position.z);
          transform.LookAt(lookAt);

          float distanceToEnemy = Vector3.Distance(transform.position, position);
          if(currentAbility.rangeType == RangeType.Small && distanceToEnemy > 5){
            return;
          }
          if(currentAbility.rangeType == RangeType.Medium && distanceToEnemy > 10){
            return;
          }
          ExecuteAbility(currentAbility, null, position);
        }
      }
      isSelectingTarget = true;
      currentAbility = abilities.Find(a => a.abilityType == AbilityType.Basic);
    }
  }

  public void UseAbility(string abilityName)
  {
    var ability = abilities.Find(a => a.abilityName == abilityName);
    if (ability != null)
    {
      if (ability.isUnlocked && !ability.isOnCooldown)
      {
        if (ability.activationType == ActivationType.Instant)
        {
          ability.Use();
          defensiveW = false;
          wildCardQ = false;
          ultimateE = false;
        }
        else if (ability.activationType == ActivationType.SelectEnemy || ability.activationType == ActivationType.SelectPosition)
        {
          isSelectingTarget = true;
          currentAbility = ability;
        }
      }
      else
      {
        defensiveW = false;
        wildCardQ = false;
        ultimateE = false;
      }
    }
  }

  void ExecuteAbility(Ability ability, GameObject enemy, Vector3? position)
  {
    ability.Use(enemy, position);
    defensiveW = false;
    wildCardQ = false;
    ultimateE = false;
  }

  public void UnlockAbility(string abilityName)
  {
    var ability = abilities.Find(a => a.abilityName == abilityName);
    if (ability != null && !ability.isUnlocked && playerStats.abilityPoints > 0)
    {
      ability.isUnlocked = true;
      playerStats.abilityPoints--;
      FindObjectOfType<HUDManager>().UpdateHUD();
    }
  }

  public Ability GetAbility(string abilityName)
  {
    return abilities.Find(a => a.abilityName == abilityName);
  }

  public PlayerStats GetPlayerStats()
  {
    return playerStats;
  }
}
