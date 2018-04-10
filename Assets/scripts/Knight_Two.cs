using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_Two : MonoBehaviour
{

    public float speed; // Player's movement speed in units
    private Vector2 lastMove;

    private float currentSpeed;
    private Animator anim;
    private Rigidbody2D body;
    private bool moving;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;
    private Vector3 moveDirection;
    public float maxFollowRange = 10f;

    private GameObject thePlayer;
    private Vector2 move;

    // Use this for initialization
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody2D>();

        lastMove = new Vector2(0f, -1f);

        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = body.velocity;
        if (move.x > move.y) move = new Vector2(move.x, 0f);
        else move = new Vector2(0f, move.y);
        lastMove = new Vector2(move.x, move.y);
        if (moving)
        {
            timeToMoveCounter -= Time.deltaTime;
            //get the direction closest to the player
            Vector3 moveDirection = Player.GetPosition() - transform.position;

            float distance = moveDirection.magnitude;
            if (distance > maxFollowRange)
            {
                moving = false;
                return;
            }
            body.AddForce(new Vector2(moveDirection.x * speed, moveDirection.y * speed));
            
            if (timeToMoveCounter < 0f)
            {
                moving = false;
                timeBetweenMoveCounter = timeBetweenMove;
            }
        }
        else
        {
            timeBetweenMoveCounter -= Time.deltaTime;
            //myRigidBody.velocity = Vector2.zero;
            if (timeBetweenMoveCounter < 0f)
            {
                moving = true;
                timeToMoveCounter = timeToMove;
            }
        }
        
        anim.SetFloat("MoveX", move.x);
        anim.SetFloat("MoveY", move.y);
        anim.SetBool("Moving", moving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }
}
