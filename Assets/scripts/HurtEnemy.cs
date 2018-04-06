using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{

    public int damage;
    public GameObject damageBurst;
    public Transform hitpoint;
    public GameObject damageNumber;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            int hit = damage + PlayerStats.attack;
            other.gameObject.GetComponent<EnemyHealthManager>().HurtEnemy(hit);
            //Instantiate(damageBurst, hitpoint.position, hitpoint.rotation);
            var clone = (GameObject)Instantiate(damageNumber, hitpoint.position, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingNumber>().damage = hit;
        }
    }
}
