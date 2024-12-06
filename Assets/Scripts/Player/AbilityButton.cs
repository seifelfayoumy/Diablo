// AbilityButton.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityButton : MonoBehaviour
{
    public string abilityName;
    public Button button;
    public Image cooldownImage;
    public TextMeshProUGUI cooldownText;
    public Image lockIcon;

    private AbilityManager abilityManager;

    void Start()
    {
        abilityManager = FindObjectOfType<AbilityManager>();
        button.onClick.AddListener(OnButtonClick);
        UpdateButton();
    }

    void Update()
    {
        UpdateCooldownUI();
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
                    abilityManager.UseAbility(abilityName);
                }
                else
                {
                    // Attempt to unlock the ability
                    if (abilityManager.GetPlayerStats().abilityPoints > 0)
                    {
                        abilityManager.UnlockAbility(abilityName);
                        // Optionally, play an unlock sound or animation
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
            button.interactable = isUnlocked;
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
                    cooldownImage.fillAmount = ability.GetCooldownRemaining() / ability.cooldown;
                    cooldownText.text = Mathf.CeilToInt(ability.GetCooldownRemaining()).ToString();
                    button.interactable = false;
                }
                else
                {
                    cooldownImage.fillAmount = 0f;
                    cooldownText.text = "";
                    bool isUnlocked = ability.isUnlocked;
                    button.interactable = isUnlocked;
                }
            }
        }
    }
}
