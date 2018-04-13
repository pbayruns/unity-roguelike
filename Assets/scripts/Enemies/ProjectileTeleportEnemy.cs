using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTeleportEnemy : BaseEnemy {

    public float maxTriggerRange = 10f;
    public float checkTriggerFrequency = 0.2f;
    private float tilCheckTrigger = 0f;
    private float distance = 0f;

    public float projectileSpeed = 6f;
    public float focusTime = 0.5f;
    private float tilStopFocus;

    public float attackTime = 0.5f;
    private float tilStopAttack;

    public float teleportTime = 0.5f;
    private float tilStopTeleport;

    private bool attacking = false;
    private bool teleporting = false;
    private bool focusingAttack = false;
    private bool focusingTeleport = false;

    public GameObject projectile_left;
    public GameObject projectile_up;

    private Vector2 idleDirection = Vector2.zero;
    private Vector3 toPlayer = Vector3.zero;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        tilStopAttack = attackTime;
        tilStopFocus = focusTime;
        tilStopTeleport = 0f;

        tilStop = movingTime;
        tilNextMove = stillTime;
    }

    new void Update()
    {
        base.Update();
    }

    protected override void SetMovement()
    {

        // dont check player position every frame for performance
        // I think this is more efficient
        tilCheckTrigger -= Time.deltaTime;
        if (tilCheckTrigger <= 0)
        {
            tilCheckTrigger = checkTriggerFrequency;
            //get the direction closest to the player
            toPlayer = Player.GetPosition() - base.transform.position;
            distance = moveDirection.magnitude;
        }

        bool inRange = (distance < maxTriggerRange);

        // Follow the player if we're in range, or if 
        // the player was in range {{followTime}} 
        // seconds ago or less. Otherwise, do idle movement.

        // Standing idle -> player walks up 
        //  -> attention -> attacks 
        //  -> attention -> teleports
        if (inRange 
            || focusingAttack || focusingTeleport 
            || attacking || teleporting)
        {
            if (tilStopTeleport <= 0)
            {
                teleporting = false;
                focusingAttack = true;
                tilStopFocus = focusTime;
                tilStopTeleport = teleportTime;
            }

            if (focusingAttack)
            {
                tilStopFocus -= Time.deltaTime;
                if(tilStopFocus <= 0)
                {
                    focusingAttack = false;
                    attacking = true;
                    tilStopAttack = attackTime;
                    Attack();
                }
            }
            else if (attacking)
            {
                tilStopAttack -= Time.deltaTime;
                if (tilStopAttack <= 0)
                {
                    attacking = false;
                    focusingTeleport = true;
                    tilStopFocus = focusTime;
                }
            }
            else if (focusingTeleport)
            {
                tilStopFocus -= Time.deltaTime;
                if (tilStopFocus <= 0)
                {
                    focusingTeleport = false;
                    teleporting = true;
                    tilStopAttack = attackTime;
                }
            }
            else if (teleporting)
            {
                tilStopTeleport -= Time.deltaTime;
                if (tilStopTeleport <= 0)
                {
                    teleporting = false;
                    focusingAttack = true;
                    tilStopFocus = focusTime;
                }
            }
            base.moveDirection = toPlayer.normalized;
            base.lastMove = moveDirection;
            base.move = Vector2.zero;
            base.moving = false;
        }
        else
        {
            IdleMovement();
        }

        base.anim.SetBool("Focusing", focusingAttack || focusingTeleport);
        base.anim.SetBool("Attacking", attacking);
        base.anim.SetBool("Teleporting", teleporting);
    }

    void Attack()
    {
        //spawn a projectile here
        Vector3 pos = gameObject.transform.position;
        Vector3 norm = toPlayer.normalized;
        Vector2 velocity = new Vector2(norm.x * projectileSpeed, norm.y * projectileSpeed);
        ProjectileManager.CreateProjectile(projectile_up, pos, velocity, Quaternion.Euler(norm));

    }

    void IdleMovement()
    {
        if (base.moving)
        {
            tilStop -= Time.deltaTime;
            base.move = new Vector2(idleDirection.x * base.speed, idleDirection.y * base.speed);
            if (tilStop < 0f)
            {
                base.lastMove = base.move;
                base.moving = false;
                tilNextMove = stillTime;
            }
        }
        else
        {
            tilNextMove -= Time.deltaTime;
            base.move = Vector2.zero;
            if (tilNextMove < 0f)
            {
                base.moving = true;
                idleDirection = MovementUtil.GetRandomDirection();
                tilStop = movingTime;
            }
        }
    }

    void ResetIdleMovement()
    {
        base.moving = false;
        base.lastMove = base.move;
        tilNextMove = stillTime;
        idleDirection = MovementUtil.GetRandomDirection();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ResetIdleMovement();
    }
}
