using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public float speed; // Player's movement speed in units
    protected float currentSpeed;

    protected Animator anim;
    protected Rigidbody2D body;
    protected bool moving;

    protected bool knockback;
    public float knockbackTime = 0.5f;
    public float knockbackForce = 500f;
    protected float tilStopKnockback;

    protected Vector3 moveDirection;
    protected Vector2 move;
    protected Vector2 lastMove;

    public float movingTime = 1f;
    protected float tilStop;

    public float stillTime = 1f;
    protected float tilNextMove;

    // Use this for initialization
    protected virtual void Start()
    {
        Init();
    }

    protected void Init()
    {
        tilStop = movingTime;
        tilNextMove = stillTime;
        tilStopKnockback = knockbackTime;

        tilNextMove = Random.Range(stillTime * 0.75f, stillTime * 1.25f);
        tilStop = Random.Range(movingTime * 0.75f, movingTime * 1.25f);

        anim = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody2D>();
        lastMove = new Vector2(0f, -1f);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (GameManager.EnemiesPaused) return;
        if (knockback)
        {
            if (tilStopKnockback > 0)
            {
                tilStopKnockback -= Time.deltaTime;
                return;
            }
            else
            {
                knockback = false;
            }
        }
        else
        {
            SetMovement();
        }

        body.velocity = move;
        SetAnimationParams(move, lastMove, moving);
    }

    void SetAnimationParams(Vector2 movement, Vector2 lastMovement, bool isMoving)
    {
        anim.SetFloat("MoveX", movement.x);
        anim.SetFloat("MoveY", movement.y);
        anim.SetBool("Moving", isMoving);
        anim.SetFloat("LastMoveX", lastMovement.x);
        anim.SetFloat("LastMoveY", lastMovement.y);
    }

    protected virtual void SetMovement()
    {
        if (moving)
        {
            tilNextMove -= Time.deltaTime;
            move = moveDirection;
            if (tilNextMove < 0f)
            {
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
                tilNextMove = stillTime;

                float randX = Random.Range(-1f, 1f) * speed;
                float randY = Random.Range(-1f, 1f) * speed;
                moveDirection = new Vector3(randX, randY, 0f);
            }
        }
    }

    public void Knockback(Vector2 direction)
    {
        direction = direction.normalized;
        Vector3 force = new Vector3(direction.x * knockbackForce, direction.y * knockbackForce, 0f);
        body.AddForce(force);
        knockback = true;
        tilStopKnockback = knockbackTime;
    }
}
