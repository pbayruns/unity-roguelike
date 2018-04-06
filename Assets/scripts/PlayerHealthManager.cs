﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{

    public static int maxHP = PlayerStats.hp;
    public static int currentHP = PlayerStats.hp;

    public static bool flashing = false;
    public static float flashTime = 0.75f;
    private static float flashCounter = flashTime;

    private SpriteRenderer playerSprite;
    private SpriteRenderer[] childSprites;

    // Use this for initialization
    void Start()
    {
        currentHP = maxHP;
        playerSprite = GetComponent<SpriteRenderer>();
        childSprites = GameObject.Find("Weapon").GetComponentsInChildren<SpriteRenderer>();
        HUD.UpdateHPDisplay(currentHP, maxHP);
    }

    // Update is called once per frame
    void Update()
    {

        if (currentHP <= 0)
        {
            //GameManager.GameOver();
            gameObject.SetActive(false);
        }
        if (flashing)
        {
            Color RGB = playerSprite.color;
            Color invisible = new Color(RGB.r, RGB.g, RGB.b, 0f);
            Color visible = new Color(RGB.r, RGB.g, RGB.b, 1f);
            Color opacity = visible;

            if (flashCounter > flashTime * .66f)
            {
                opacity = invisible;
            }
            else if (flashCounter > flashTime * .33f)
            {
                opacity = visible;
            }
            else if (flashCounter > 0)
            {
                opacity = invisible;
            }
            else
            {
                flashing = false;
                opacity = visible;
            }
            playerSprite.color = opacity;
            for(int i = 0; i < childSprites.Length; i++)
            {
                childSprites[i].color = opacity;
            }
            flashCounter -= Time.deltaTime;
        }
    }

    public static void HurtPlayer(int damage)
    {
        currentHP -= damage;
        flashing = true;
        flashCounter = flashTime;
        SFXManager.PlaySFX(SFX_TYPE.PLAYER_HURT);
        HUD.UpdateHPDisplay(currentHP, maxHP);
    }

    public static void SetMaxHP(int hp)
    {
        if (hp < 1) return;
        if(hp > maxHP)
        {
            int hpUp = hp - maxHP;
            maxHP = hp;
            currentHP += hpUp;
        }
        else
        {
            maxHP = hp;
            if (currentHP > maxHP) currentHP = maxHP;
        }
        HUD.UpdateHPDisplay(currentHP, maxHP);
    }
}