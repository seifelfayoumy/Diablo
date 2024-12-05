using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int level = 1;
    public int currentXP = 0;
    public int requiredXP = 100; // Initial XP required to level up
    public int abilityPoints = 0;
    public int maxHP = 100;
    public int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void GainXP(int amount)
    {
        currentXP += amount;

        if (currentXP >= requiredXP)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        abilityPoints++;
        maxHP += 100;  // Increase max HP per level
        currentHP = maxHP;  // Heal the player when leveling up
        requiredXP = 100 * level;
    }
}
