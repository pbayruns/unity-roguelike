using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerEnemy : BaseEnemy
{
   
    public float maxFollowRange = 10f;

    public float followTime = 3f;
    private float tilStopFollow;

    private Vector2 idleDirection = Vector2.zero;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        tilStopFollow = followTime;
        tilStop = movingTime;
        tilNextMove = stillTime;
    }

    new void Update()
    {
        base.Update();
    }

    protected override void SetMovement()
    {
        //get the direction closest to the player
        Vector3 moveDirection = Player.GetPosition() - base.transform.position;
        float distance = moveDirection.magnitude;

        bool inRange = (distance < maxFollowRange);
        bool followPlayer = false;

        // Follow the player if we're in range, or if 
        // the player was in range {{followTime}} 
        // seconds ago or less. Otherwise, do idle movement.
        if (inRange)
        {
            tilStopFollow = followTime;
            followPlayer = true;
        }
        else if (tilStopFollow > 0)
        {
            tilStopFollow -= Time.deltaTime;
            followPlayer = true;
        }

        if (followPlayer)
        {
            moveDirection = moveDirection.normalized;
            base.move = new Vector2(moveDirection.x * base.speed, moveDirection.y * base.speed);
            base.moving = true;
        }
        else
        {
            IdleMovement();
        }
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
