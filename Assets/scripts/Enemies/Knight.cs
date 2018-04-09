using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour {


    public float moveSpeed;

    private Rigidbody2D myRigidBody;

    private bool moving;
    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;
    private Vector3 moveDirection;
    public float maxFollowRange = 10f;
    public float waitToReload;
    private bool reloading;

    private GameObject thePlayer;
    // Use this for initialization
    void Start()
    {
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            timeToMoveCounter -= Time.deltaTime;
            //get the direction closest to the player
            Vector3 moveDirection = Player.GetPosition() - transform.position;
            float distance = moveDirection.magnitude;
            if(distance > maxFollowRange)
            {
                return;
            }
            myRigidBody.AddForce(new Vector2(moveDirection.x * 5, moveDirection.y * 5));
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
    }
}