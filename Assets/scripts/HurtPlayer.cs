using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{

    public int damage;
    public GameObject damageDisplay;
    public static float damageCooldown = 0.75f;
    private bool canDamage;

    private void Start()
    {
        canDamage = true;
        damageCooldown = 0.75f;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && canDamage)
        {
            canDamage = false;
            Invoke("SetDamageable", damageCooldown);
            int hit = damage - PlayerStats.defense;
            hit = (hit < 0) ? 0 : hit;
            PlayerHealthManager.HurtPlayer(hit);
            SFXManager.PlaySFX(SFX_TYPE.PLAYER_HURT);
            var clone = (GameObject)Instantiate(damageDisplay, Player.instance.transform.position, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingNumber>().damage = hit;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && canDamage)
        {
            canDamage = false;
            Invoke("SetDamageable", damageCooldown);
            int hit = damage - PlayerStats.defense;
            hit = (hit < 0) ? 0 : hit;
            PlayerHealthManager.HurtPlayer(hit);
            SFXManager.PlaySFX(SFX_TYPE.PLAYER_HURT);
            var clone = (GameObject)Instantiate(damageDisplay, Player.instance.transform.position, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingNumber>().damage = hit;
        }
    }


    void SetDamageable()
    {
        Debug.Log("setting damageable");
        canDamage = true;
    }
}
