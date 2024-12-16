using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
  public Slider hpSlider;
  public TextMeshProUGUI hpText;

  public Slider xpSlider;
  public TextMeshProUGUI xpText;

  public TextMeshProUGUI levelText;
  public TextMeshProUGUI abilityPointsText;
  public TextMeshProUGUI healingPotionsText;
  public TextMeshProUGUI runeFragmentsText;

  public HealthBar bossHealthBar;
  public HealthBar bossShieldBar;

  public PlayerStats playerStats;
  public PlayerHealth playerHealth;

  public GameObject abilitiesPanelBarbarian;
  public GameObject abilitiesPanelSorcerer;
  public GameObject abilitiesPanelRogue;

  public Button healPotionButton;

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
  }

  void Update()
  {
    UpdateHP();
    UpdateXP();
  }

  public void UpdateHP()
  {
    hpSlider.maxValue = playerStats.maxHP;
    hpSlider.value = playerStats.currentHP;
    hpText.text = $"{playerStats.currentHP} / {playerStats.maxHP}";
  }

  public void UpdateXP()
  {
    xpSlider.maxValue = playerStats.requiredXP;
    xpSlider.value = playerStats.currentXP;
    xpText.text = $"{playerStats.currentXP} / {playerStats.requiredXP}";
  }

  public void UpdateHUD()
  {
    levelText.text = $"Level: {playerStats.level}";
    abilityPointsText.text = $"Ability Points: {playerStats.abilityPoints}";
    healingPotionsText.text = $"Potions: {playerStats.healingPotions}";
    runeFragmentsText.text = $"Runes: {playerStats.runeFragments}";
  }

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

  public void UseHealingPotion()
  {
    if (playerStats.healingPotions > 0 && playerStats.currentHP < playerStats.maxHP)
    {
      int healAmount = Mathf.RoundToInt(playerStats.maxHP * 0.5f);
      playerHealth.Heal(healAmount);
      playerStats.healingPotions--;
      UpdateHUD();
    }
  }
}
