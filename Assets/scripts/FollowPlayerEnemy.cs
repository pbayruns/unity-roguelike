using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerEnemy : MonoBehaviour
{
    public float speed; // Player's movement speed in units
    private float currentSpeed;

    private Animator anim;
    private Rigidbody2D body;
    private bool moving;

    private bool knockback;
    private Vector2 knockbackMovement;
    public float knockbackTime = 0.5f;
    public float knockbackSpeed = 5;
    private float tilStopKnockback;

    private Vector3 moveDirection;
    private Vector2 move;
    private Vector2 lastMove;

    public float maxFollowRange = 10f;

    public float followTime = 3f;
    private float tilStopFollow;

    public float movingTime = 1f;
    private float tilStop;

    public float stillTime = 1f;
    private float tilNextMove;

    private Vector2 idleDirection = Vector2.zero;

    // Use this for initialization
    void Start()
    {
        tilStopFollow = followTime;
        tilStop = movingTime;
        tilNextMove = stillTime;
        tilStopKnockback = knockbackTime;

        anim = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody2D>();
        lastMove = new Vector2(0f, -1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (knockback)
        {
            move = knockbackMovement;
            tilStopKnockback -= Time.deltaTime;
            if(tilStopKnockback >= 0)
            {
                knockback = false;
            }
        }
        else
        {
            SetMovement();
        }
        

        body.velocity = move;
        anim.SetFloat("MoveX", move.x);
        anim.SetFloat("MoveY", move.y);
        anim.SetBool("Moving", moving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }

    void SetMovement()
    {
        //get the direction closest to the player
        Vector3 moveDirection = Player.GetPosition() - transform.position;
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
            move = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
            moving = true;
        }
        else
        {
            IdleMovement();
        }
    }

    public static Vector2 GetRandomDirection()
    {
        int direction = Random.Range(0, 3);
        switch (direction)
        {
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.down;
            case 2:
                return Vector2.left;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    void IdleMovement()
    {
        if (moving)
        {
            tilStop -= Time.deltaTime;
            move = new Vector2(idleDirection.x * speed, idleDirection.y * speed);
            if (tilStop < 0f)
            {
                lastMove = move;
                moving = false;
                tilNextMove = stillTime;
            }
        }
        else
        {
            tilNextMove -= Time.deltaTime;
            move = Vector2.zero;
            if (tilNextMove < 0f)
            {
                moving = true;
                idleDirection = GetRandomDirection();
                tilStop = movingTime;
            }
        }
    }

    void ResetIdleMovement()
    {
        moving = false;
        lastMove = move;
        tilNextMove = stillTime;
        idleDirection = GetRandomDirection();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ResetIdleMovement();
    }

    public void Knockback(Vector2 direction)
    {
        knockbackMovement = new Vector2(direction.x * knockbackSpeed, direction.y * knockbackSpeed);
        knockback = true;
        tilStopKnockback = knockbackTime;
    }
}
