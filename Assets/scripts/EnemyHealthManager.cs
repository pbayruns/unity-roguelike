using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    public int MaxHealth;
    public int CurrentHealth;

    public int expToGive;

    // Use this for initialization
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHealth <= 0)
        {
            SFXManager.PlaySFX(SFX_TYPE.DEATH_EXPLOSION);
            Destroy(gameObject);
            PlayerStats.AddExperience(expToGive);
        }
    }

    public void HurtEnemy(int damage)
    {
        CurrentHealth -= damage;
    }

    public void SetMaxHealth()
    {
        CurrentHealth = MaxHealth;
    }
}
