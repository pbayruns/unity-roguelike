using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{

    public int damage;
    public GameObject damageDisplay;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            int hit = damage - PlayerStats.defense;
            hit = (hit < 0) ? 0 : hit;
            PlayerHealthManager.HurtPlayer(hit);
            var clone = (GameObject)Instantiate(damageDisplay, Player.instance.transform.position, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingNumber>().damage = hit;
        }
    }
}
