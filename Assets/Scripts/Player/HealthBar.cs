using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI healthText;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetHealthText(int health)
    {
        healthText.text = health + " / " + slider.maxValue;
    }
}
