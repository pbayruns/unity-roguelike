using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{

    public int damage;
    public GameObject damageBurst;
    public Transform hitpoint;
    public GameObject damageNumber;

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
            other.gameObject.GetComponent<EnemyHealthManager>().HurtEnemy(hit);
            Rigidbody2D otherBody = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 direction = Player.GetLastMove().normalized;
            otherBody.AddForce(new Vector2(direction.x * 700, direction.y * 700));
            //Instantiate(damageBurst, hitpoint.position, hitpoint.rotation);
            SFXManager.PlaySFX(SFX_TYPE.ENEMY_HURT);
            var clone = (GameObject)Instantiate(damageNumber, hitpoint.position, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingNumber>().damage = hit;
        }
    }
}
