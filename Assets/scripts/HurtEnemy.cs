using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{

    public int damage;
    public GameObject damageBurst;
    public Transform hitpoint;
    public GameObject damageNumber;
    public static Color damageColor = new Color(0.7f, 0.35f, 0.3f, 1f);
    public static Color normalColor = new Color(1f, 1f, 1f, 1f);
    //public Color InvertColor(Color color)
    //{
    //    return new Color(1f - color.r, 1f - color.g, 1f - color.b);
    //}

    //public void InvertRendererColors(SpriteRenderer render)
    //{
    //    Color[] colors = render.sprite.texture.GetPixels();
    //    for (int x = 0; x < colors.Length; x++)
    //    {
    //        InvertColor(colors[x]);
    //    }
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            int hit = damage + PlayerStats.attack;
            if (hit > 0)
            {
                SFXManager.PlaySFX(SFX_TYPE.ENEMY_HURT);
                DealDamage(other, hit);
                Knockback(other);
                SpriteRenderer sprite = other.GetComponent<SpriteRenderer>();
                DamageFlash(sprite);
            }
            else
            {
                SFXManager.PlaySFX(SFX_TYPE.ATTACK_FAILED);
            }

            //Instantiate(damageBurst, hitpoint.position, hitpoint.rotation);

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
        StartCoroutine(SetColor(sprite, normalColor, flashTime * 5));
    }

    public IEnumerator SetColor(SpriteRenderer sprite, Color color, float delayTime = 0.2f)
    {
        yield return new WaitForSeconds(delayTime);
        if(sprite != null) sprite.color = color;
    }

    int DealDamage(Collider2D other, int hit)
    {
        other.gameObject.GetComponent<EnemyHealthManager>().DealDamage(hit);
        var clone = (GameObject)Instantiate(damageNumber, hitpoint.position, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingNumber>().damage = hit;
        return hit;
    }
    public static void Knockback(Collider2D other)
    {
        Rigidbody2D otherBody = other.gameObject.GetComponent<Rigidbody2D>();
        Vector2 direction = Player.GetLastMove().normalized;
        otherBody.AddForce(new Vector2(direction.x * 700, direction.y * 700));
    }
}
