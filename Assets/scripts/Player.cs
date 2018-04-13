using UnityEngine;
using System.Collections;
using UnityEngine.UI;   //Allows us to use UI.
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance = null;				//Static instance of Player which allows it to be accessed by any other script.

    public float restartLevelDelay = 0.3f;		//Delay time in seconds to restart level.
    public float speed; // Player's movement speed in units
    public Vector2 lastMove;
    public float attackTime;
    public string startPoint;
    public bool canMove;
    public float sprintModifier = 1.5f;

    private float currentSpeed;
    private Animator anim;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private bool moving;
    private Vector2 moveInput;
    private bool attacking;
    private float attackTimeCounter;
    private bool paused = false;
    public AudioSource stairsSound;

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

        anim = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        canMove = true;
        lastMove = new Vector2(0f, -1f);
    }

    public static Vector2 GetLastMove()
    {
        return instance.lastMove;
    }

    public static void Move(Vector3 destination)
    {
        instance.gameObject.transform.position = destination;
    }

    public static Vector3 GetPosition()
    {
        if (instance != null) return instance.gameObject.transform.position;
        else return Vector3.zero;
    }

    public static CameraController GetCamera()
    {
        return instance.GetComponentInChildren<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            return;
        }
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


        if (attacking)
        {
            attackTimeCounter -= Time.deltaTime;
            if (attackTimeCounter <= 0)
            {
                attacking = false;
                anim.SetBool("IsAttacking", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            if(attackTimeCounter <= 0)
            {
                attackTimeCounter = attackTime;
                attacking = true;
                body.velocity = Vector2.zero;
                anim.SetBool("IsAttacking", true);
                anim.SetTrigger("Attack");
                SFXManager.PlaySFX(SFX_TYPE.SWORD_ATTACK);
            }
        }

        anim.SetFloat("MoveX", xIn);
        anim.SetFloat("MoveY", yIn);
        anim.SetBool("IsMoving", moving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        bool ePressed = Input.GetKeyDown(KeyCode.E);
        if (other.tag == "Stairs")
        {
            HUD.ShowInfoText("<Press E to descend>");
            if (ePressed)
            {
                HUD.HideInfoText();
                SFXManager.PlaySFX(SFX_TYPE.STAIRS_DOWN);
                Restart();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool ePressed = Input.GetKeyDown(KeyCode.E);
        if (other.tag == "Stairs")
        {
            HUD.ShowInfoTextTimed("<Press E to descend>", 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Stairs")
        {
            HUD.HideInfoText();
        }
    }


    //Restart reloads the scene when called.
    private void Restart()
    {
        //Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
        //and not load all the scene object in the current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public static void EnableHitbox(float delay = 0f)
    {
        instance.Invoke("EnableHitbox", delay);
    }

    private void EnableHitbox()
    {
        instance.boxCollider.enabled = true;
    }

    public static void DisableHitbox()
    {
        instance.boxCollider.enabled = false;
    }

    public static void Pause()
    {
        instance.paused = true;
    }

    public static void Resume()
    {
        instance.paused = false;
    }
}

