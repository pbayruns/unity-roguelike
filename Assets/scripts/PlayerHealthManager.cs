using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{

    public static int maxHP = PlayerStats.hp;
    public static int currentHP = PlayerStats.hp;

    public static bool flashing;
    public static float flashTime;
    private static float flashCounter;

    private SpriteRenderer playerSprite;

    // Use this for initialization
    void Start()
    {
        currentHP = maxHP;
        playerSprite = GetComponent<SpriteRenderer>();
        HUD.UpdateHPDisplay(currentHP, maxHP);
    }

    // Update is called once per frame
    void Update()
    {

        if (currentHP <= 0)
        {
            gameObject.SetActive(false);
        }
        if (flashing)
        {
            Color RGB = playerSprite.color;
            Color invisible = new Color(RGB.r, RGB.g, RGB.b, 0f);
            Color visible = new Color(RGB.r, RGB.g, RGB.b, 1f);
            Color showColor = visible;

            if (flashCounter > flashTime * .66f)
            {
                showColor = invisible;
            }
            else if (flashCounter > flashTime * .33f)
            {
                showColor = visible;
            }
            else if (flashCounter > 0)
            {
                showColor = invisible;
            }
            else
            {
                flashing = false;
                showColor = visible;
            }
            playerSprite.color = showColor;
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
