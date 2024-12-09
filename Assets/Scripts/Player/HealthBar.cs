// HealthBar.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthBar : MonoBehaviour
{
    public Slider slider; // Reference to the Slider UI component
    public Gradient gradient; // Gradient to represent health color change

    public TextMeshProUGUI healthText; // Reference to the Text UI component

    

    // Sets the maximum health
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

    }

    // Sets the current health
    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetHealthText(int health)
    {
        healthText.text = health + " / " + slider.maxValue;
    }
}
