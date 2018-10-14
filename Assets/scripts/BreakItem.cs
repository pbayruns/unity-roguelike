using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakItem : MonoBehaviour
{
    public int damage;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "WorldObject")
        {
            Debug.Log("yorp");
            Breakable breakableObj = other.GetComponent<Breakable>();
            if(breakableObj != null){
                // break the other obj
                int hit = damage + PlayerStats.attack;
                breakableObj.Damage(hit);
            }
        }
    }

}