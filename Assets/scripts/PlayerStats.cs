using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance = null;
    public const int STATS_PER_LVL = 3;
    public static int currentLevel = 1;
    public static int currentExp = 0;
    public static int availableStatpoints = 0;

    public static int strength = 1;
    public static int agility = 1;
    public static int wisdom = 1;
    public static int attack = 3;
    public static int defense = 0;
    public static int hp = 20;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public static void Reset()
    {
        currentLevel = 1;
        currentExp = 0;
        availableStatpoints = 0;
        hp = 15;
        attack = 3;
        defense = 0;
        strength = 1;
        agility = 1;
        wisdom = 1;
    }

    public static void AddStrength(int amt = 1)
    {
        strength += amt;
    }

    public static void AddAgility(int amt = 1)
    {
        agility += amt;
    }

    public static void AddWisdom(int amt = 1)
    {
        wisdom += amt;
    }

    public void Start()
    {
        HUD.UpdateLevelDisplay(currentLevel, currentExp, GetXPToNextLvl());
    }

    public static int GetHPForLevel(int level)
    {
        int hp = ((level - 1) * 10) + 15;
        double val = Math.Ceiling(hp/4f);
        return (int) (4 * val);
    }

    public static int GetDefenseForLevel(int level)
    {
        return (int)(level / 2.5f + 1);
    }

    public static int GetAttackForLevel(int level)
    {
        return (int)(level / 2.5f + 1);
    }

    public static int GetXPForLevel(int level)
    {
        return (int)Mathf.Round((12 * (Mathf.Pow(level, 3))) / 5);
    }

    public static void AddExperience(int xp)
    {
        currentExp += xp;
        HUD.UpdateLevelDisplay(currentLevel, currentExp, GetXPToNextLvl());
        PlayerNotification.DisplayXPNotification(xp);
        if (currentExp >= GetXPForLevel(currentLevel + 1))
        {
            LevelUp();
        }
    }

    public static int GetXPToNextLvl()
    {
        return GetXPForLevel(currentLevel + 1) - currentExp;
    }

    public static void LevelUp()
    {
        currentLevel++;
        int hpDif = hp;
        int atkDif = attack;
        int defDif = defense;

        availableStatpoints += STATS_PER_LVL;

        hp = GetHPForLevel(currentLevel);
        attack = GetAttackForLevel(currentLevel);
        defense = GetDefenseForLevel(currentLevel);

        hpDif = hp - hpDif;
        atkDif = attack - atkDif;
        defDif = defense - defDif;

        PlayerHealthManager.SetMaxHP(hp);
        SFXManager.PlaySFX(SFX_TYPE.LEVEL_UP);
        HUD.RefreshDataDisplay();
        PlayerNotification.DisplayLevelUpNotification(currentLevel, atkDif, defDif, hpDif);
    }
}
