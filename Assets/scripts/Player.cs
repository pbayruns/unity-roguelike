using UnityEngine;
using System.Collections;
using UnityEngine.UI;   //Allows us to use UI.
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance = null;				//Static instance of Player which allows it to be accessed by any other script.

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
    private bool attacking;
    private float attackTimeCounter;

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();

        canMove = true;
        lastMove = new Vector2(0f, -1f);
    }

    public static void Move(Vector3 destination)
    {
        instance.gameObject.transform.position = destination;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Stairs")
        {
            Invoke("Restart", restartLevelDelay);
        }
    }

    //Restart reloads the scene when called.
    private void Restart()
    {
        //Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
        //and not load all the scene object in the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}

