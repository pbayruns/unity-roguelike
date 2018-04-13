using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance = null;

    public static int currentLevel = 1;
    public static int currentExp = 0;

    public static int hp = 15;
    public static int attack = 3;
    public static int defense = 0;

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

    public void Start()
    {
        HUD.UpdateLevelDisplay(currentLevel, currentExp);
    }

    public static int GetHPForLevel(int level)
    {
        return (level - 1 * 20) + 15;
    }

    public static int GetDefenseForLevel(int level)
    {
        return (int) (level / 2.5f + 1);
    }

    public static int GetAttackForLevel(int level)
    {
        return (int)(level / 2.5f + 1);
    }

    public static int GetXPForLevel(int level)
    {
        return (int) Mathf.Round((12 * (Mathf.Pow(level, 3))) / 5);
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
