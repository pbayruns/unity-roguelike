using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    public int MaxHealth;
    public int CurrentHealth;
    private PausableRigidBody2D pauseBody;
    private Rigidbody2D rigidBody;
    private Collider2D bodyCollider;

    public int expToGive;
    public ParticleSystem smokeburst;
    public Enemy enemyType;

    // Use this for initialization
    void Start()
    {
        CurrentHealth = MaxHealth;
        rigidBody = GetComponent<Rigidbody2D>();
        pauseBody = GetComponent<PausableRigidBody2D>();
        bodyCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 999999;
            HurtEnemy.Knockback(bodyCollider);
            Invoke("EnemyDeath", 0.1f);
        }
    }

    public void EnemyDeath()
    {
        pauseBody.Pause();
        rigidBody.velocity = Vector2.zero;
        Instantiate(smokeburst, PosAtZ(-1f), Quaternion.Euler(Vector3.zero));
        Invoke("DestroyEnemy", 0.3f);
        SFXManager.PlaySFX(SFX_TYPE.DEATH_EXPLOSION);
        PlayerStats.AddExperience(expToGive);
    }

    public Vector3 PosAtZ(float z)
    {
        Vector3 pos = gameObject.transform.position;
        pos.z = z;
        return pos;
    }
    public void DestroyEnemy()
    {
        LootManager.DropLoot(enemyType, PosAtZ(-0.5f));
        Destroy(gameObject);
    }

public void DealDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    public void SetMaxHealth()
    {
        CurrentHealth = MaxHealth;
    }
}
