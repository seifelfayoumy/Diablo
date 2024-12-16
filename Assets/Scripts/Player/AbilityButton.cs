using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityButton : MonoBehaviour
{
  public string abilityName;
  public Button button;
  public Slider cooldownImage;
  public TextMeshProUGUI cooldownText;
  public Image lockIcon;

  private AbilityManager abilityManager;

  void Start()
  {
    abilityManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();
    button.onClick.AddListener(OnButtonClick);
    UpdateButton();
  }

  void Update()
  {
    UpdateCooldownUI();
    UpdateButton();

  }

  void OnButtonClick()
  {
    if (abilityManager != null)
    {
      var ability = abilityManager.GetAbility(abilityName);
      if (ability != null)
      {
        if (ability.isUnlocked)
        {
          //abilityManager.UseAbility(abilityName);
        }
        else
        {
          if (abilityManager.GetPlayerStats().abilityPoints > 0)
          {
            abilityManager.UnlockAbility(abilityName);
          }
        }
      }
    }
  }

  void UpdateButton()
  {
    if (abilityManager != null)
    {
      bool isUnlocked = abilityManager.GetAbility(abilityName).isUnlocked;
      lockIcon.gameObject.SetActive(!isUnlocked);
      bool canBeUnlocked = abilityManager.GetPlayerStats().abilityPoints > 0;
      button.interactable = canBeUnlocked;
    }
  }

  void UpdateCooldownUI()
  {
    if (abilityManager != null)
    {
      var ability = abilityManager.GetAbility(abilityName);
      if (ability != null)
      {
        if (ability.isOnCooldown)
        {
          cooldownImage.value = ability.GetCooldownRemaining();
          cooldownImage.maxValue = ability.cooldown;
          cooldownText.text = Mathf.CeilToInt(ability.GetCooldownRemaining()).ToString();
          button.interactable = false;
        }
        else
        {
          cooldownImage.value = 0f;
          cooldownImage.maxValue = 1f;
          cooldownText.text = "";
          bool isUnlocked = ability.isUnlocked;
          button.interactable = isUnlocked;
        }
      }
    }
  }
}
