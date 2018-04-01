using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.

public class Player : MonoBehaviour
{
    public float restartLevelDelay = 1f;		//Delay time in seconds to restart level.
    public float speed; // Player's movement speed in units
    public Vector2 lastMove;
    public float attackTime;
    public string startPoint;
    public bool canMove;
    public float sprintModifier = 1.5f;

    private float currentSpeed;
    private Animator anim;
    private Rigidbody2D body;
    private bool moving;
    private Vector2 moveInput;
    private static bool playerExists;
    private bool attacking;
    private float attackTimeCounter;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        canMove = true;
        lastMove = new Vector2(0f, -1f);
    }

    // Update is called once per frame
    void Update()
    {
        moving = false;
        if (!canMove)
        {
            body.velocity = Vector2.zero;
            return;
        }

        float xIn = Input.GetAxisRaw("Horizontal");
        float yIn = Input.GetAxisRaw("Vertical");
        if (!attacking)
        {
            moveInput = new Vector2(xIn, yIn).normalized;
            if (moveInput != Vector2.zero)
            {
                float speed = this.speed;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed *= sprintModifier;
                }
                float xVel = moveInput.x * speed;
                float yVel = moveInput.y * speed;
                body.velocity = new Vector2(xVel, yVel);
                moving = true;
                lastMove = moveInput;
            }
            else
            {
                body.velocity = Vector2.zero;
            }
        }

        if (attackTimeCounter > 0)
        {
            attackTimeCounter -= Time.deltaTime;
        }
        if (attackTimeCounter <= 0)
        {
            attacking = false;
            anim.SetBool("IsAttacking", false);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            attackTimeCounter = attackTime;
            attacking = true;
            body.velocity = Vector2.zero;
            anim.SetBool("IsAttacking", true);
        }

        anim.SetFloat("MoveX", xIn);
        anim.SetFloat("MoveY", yIn);
        anim.SetBool("IsMoving", moving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }
}

