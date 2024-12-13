// HUDManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
  // Non-Interactable HUD Elements
  public Slider hpSlider;
  public TextMeshProUGUI hpText;

  public Slider xpSlider;
  public TextMeshProUGUI xpText;

  public TextMeshProUGUI levelText;
  public TextMeshProUGUI abilityPointsText;
  public TextMeshProUGUI healingPotionsText;
  public TextMeshProUGUI runeFragmentsText;

  public HealthBar bossHealthBar; // Assign in Inspector for Boss level
  public HealthBar bossShieldBar; // Assign in Inspector for Boss shield

  // Reference to Player
  public PlayerStats playerStats;
  public PlayerHealth playerHealth;

  // Abilities Panel
  public GameObject abilitiesPanelBarbarian;
  public GameObject abilitiesPanelSorcerer;
  public GameObject abilitiesPanelRogue;

  // Healing Potion Usage
  public Button healPotionButton; // Assign the UI button for using healing potions

  void Start()
  {
    playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();




    GameObject player = GameObject.FindGameObjectWithTag("Player");

    if (player.GetComponent<Barbarian>() != null)
    {
      abilitiesPanelBarbarian.SetActive(true);
    }
    else if (player.GetComponent<Sorcerer>() != null)
    {
      abilitiesPanelSorcerer.SetActive(true);
    }
    else if (player.GetComponent<Rogue>() != null)
    {
      abilitiesPanelRogue.SetActive(true);
    }


    UpdateHUD();
    //get player stats and health components form object tagged Player

  }

  void Update()
  {
    // Continuously update HP and XP
    UpdateHP();
    UpdateXP();
    // Other HUD elements are updated upon events like level up, ability unlock, etc.
  }

  // Update Health Points in HUD
  public void UpdateHP()
  {
    hpSlider.maxValue = playerStats.maxHP;
    hpSlider.value = playerStats.currentHP;
    hpText.text = $"{playerStats.currentHP} / {playerStats.maxHP}";
  }

  // Update Experience Points in HUD
  public void UpdateXP()
  {
    xpSlider.maxValue = playerStats.requiredXP;
    xpSlider.value = playerStats.currentXP;
    xpText.text = $"{playerStats.currentXP} / {playerStats.requiredXP}";
  }

  // Update Level, Ability Points, Healing Potions, Rune Fragments
  public void UpdateHUD()
  {
    levelText.text = $"Level: {playerStats.level}";
    abilityPointsText.text = $"Ability Points: {playerStats.abilityPoints}";
    healingPotionsText.text = $"Potions: {playerStats.healingPotions}";
    runeFragmentsText.text = $"Runes: {playerStats.runeFragments}";

    // Update abilities panel lock icons if necessary
    // This can be handled by AbilityButton scripts or here based on implementation
  }

  // Optional: Update Boss Health and Shield if in Boss level
  public void UpdateBossHUD(int bossHP, int bossMaxHP, int shieldHP, int shieldMaxHP)
  {
    if (bossHealthBar != null)
    {
      bossHealthBar.SetMaxHealth(bossMaxHP);
      bossHealthBar.SetHealth(bossHP);
    }

    if (bossShieldBar != null)
    {
      bossShieldBar.SetMaxHealth(shieldMaxHP);
      bossShieldBar.SetHealth(shieldHP);
    }
  }

  // Handle Healing Potion Usage
  public void UseHealingPotion()
  {
    if (playerStats.healingPotions > 0 && playerStats.currentHP < playerStats.maxHP)
    {
      int healAmount = Mathf.RoundToInt(playerStats.maxHP * 0.5f);
      playerHealth.Heal(healAmount);
      playerStats.healingPotions--;
      UpdateHUD();
      // Optional: Play healing sound or animation
    }
  }
}
