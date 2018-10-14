using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    public ParticleSystem damageAnimation;
    public ParticleSystem breakAnimation;
    public int MaxHP;
    private int HP;

    private Sprite sprite;
    public static Color damageColor = new Color(0.7f, 0.35f, 0.3f, 1f);
    public static Color normalColor = new Color(1f, 1f, 1f, 1f);

    public void Start()
    {
        this.HP = MaxHP;
        this.sprite = GetComponent<Sprite>();
    }
    public void Damage(int hit)
    {
        Instantiate(damageAnimation, PlacementUtil.PosAtZ(-1f, gameObject), Quaternion.Euler(Vector3.zero));
        this.HP -= hit;
        if (this.HP <= 0)
        {
            this.Break();
        } else {
            SFXManager.PlaySFX(SFX_TYPE.WOOD_HIT);
        }
    }

    void DamageFlash(SpriteRenderer sprite)
    {
        
        float flashTime = 0.05f;
        sprite.color = damageColor;
        StartCoroutine(SetColor(sprite, normalColor, flashTime));
        StartCoroutine(SetColor(sprite, damageColor, flashTime * 2));
        StartCoroutine(SetColor(sprite, normalColor, flashTime * 3));
        StartCoroutine(SetColor(sprite, damageColor, flashTime * 4));
        StartCoroutine(SetColor(sprite, normalColor, flashTime * 10));
    }

    public IEnumerator SetColor(SpriteRenderer sprite, Color color, float delayTime = 0.2f)
    {
        yield return new WaitForSeconds(delayTime);
        if (sprite != null) sprite.color = color;
    }

    public void Break()
    {
        SFXManager.PlaySFX(SFX_TYPE.WOOD_BREAK);
        Instantiate(breakAnimation, PlacementUtil.PosAtZ(-1f, gameObject), Quaternion.Euler(Vector3.zero));
        Destroy(gameObject);
    }
}
