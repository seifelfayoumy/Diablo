using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int level = 1;
    public int currentXP = 0;
    public int requiredXP = 100;
    public int abilityPoints = 0;
    public int maxHP = 100;
    public int currentHP;

    public int healingPotions = 0;
    public int runeFragments = 0;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void GainXP(int amount)
    {
        if(level == 4)
            return;
        currentXP += amount;

        while (currentXP >= requiredXP)
        {
            currentXP -= requiredXP;
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        abilityPoints++;
        maxHP += 100;
        currentHP = maxHP;
        requiredXP = 100 * level;
        FindObjectOfType<HUDManager>().UpdateHUD();
    }
}
