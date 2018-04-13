using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager instance = null;
    public static int maxHP = PlayerStats.hp;
    public static int currentHP = PlayerStats.hp;

    public static bool flashing = false;
    public static float flashTime = 0.75f;
    private static float flashCounter = flashTime;

    private bool invulnerable = false;

    private SpriteRenderer playerSprite;
    private SpriteRenderer[] childSprites;

    //Singleton
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

    private void Start()
    {
        currentHP = maxHP;
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        childSprites = GameObject.Find("Weapon").GetComponentsInChildren<SpriteRenderer>();
        HUD.UpdateHPDisplay(currentHP, maxHP);
    }

    public static void MakeInvulnerable(float delay = 0f)
    {
        instance.Invoke("MakeInvulnerable", delay);
    }

    private void MakeInvulnerable()
    {
        Color RGB = playerSprite.color;
        Color ghost = new Color(RGB.r, RGB.g, RGB.b, 0.3f);
        playerSprite.color = ghost;
        invulnerable = true;
    }

    public static void MakeVulnerable(float delay = 0f)
    {
        instance.Invoke("MakeVulnerable", delay);
    }

    private void MakeVulnerable()
    {
        Color RGB = playerSprite.color;
        Color solid = new Color(RGB.r, RGB.g, RGB.b, 1f);
        playerSprite.color = solid;
        invulnerable = false;
        flashing = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (currentHP <= 0)
        {
            Start();
            GameManager.GameOver();
        }
        if (invulnerable)
        {

            return;
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
        if (instance.invulnerable) return;
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
