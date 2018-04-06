using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int currentLevel;
    public static int currentExp;

    public static int hp = 20;
    public static int attack = 3;
    public static int defense = 0;

    public static int GetHPForLevel(int level)
    {
        return level * 15;
    }

    public static int GetDefenseForLevel(int level)
    {
        return level / 3 + 1;
    }

    public static int GetAttackForLevel(int level)
    {
        return level / 2 + 1;
    }

    public static int GetXPForLevel(int level)
    {
        return (int) Mathf.Round((4 * (level ^ 3)) / 5);
    }
    public static void addExperience(int xp)
    {
        currentExp += xp;
        if (currentExp >= GetXPForLevel(currentLevel))
        {
            LevelUp();
        }
    }

    public static void LevelUp()
    {
        currentLevel++;
        hp = GetHPForLevel(currentLevel);
        attack = GetAttackForLevel(currentLevel);
        defense = GetDefenseForLevel(currentLevel);
        PlayerHealthManager.SetMaxHP(hp);
    }
}
