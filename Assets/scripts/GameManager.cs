using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;	//Time to wait before starting level, in seconds.
    public float playerStartInvulnerability = 4f;
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;
    public bool doingSetup;
    public Player player;
    private Text levelText;                                 //Text to display current level number.
    private GameObject levelImage;                          //Image to block out level as levels are being set up, background for levelText.
    private float defaultDeltaTime;
    public int level = 1; //Current level number
    //private bool doingSetup = true; //bool used to prevent Player from moving during setup.	
    public BoardCreator boardScript; //BoardManager which will set up the level.
    private PausableRigidBody2D[] RBs;
    //Awake is always called before any Start functions
    void Awake()
    {
        // Make sure there is always only one instance
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        boardScript = FindObjectOfType<BoardCreator>();
        player = Instantiate(player);
        //Instantiate(cam);
        //cam.SetFollowTarget(player.transform.gameObject);
        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //this is called only once, and the paramter tell it to be called only after the scene was loaded
    //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.level++;
        instance.InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        boardScript = FindObjectOfType<BoardCreator>();

        //While doingSetup is true the player can't move, prevent player from moving while title card is up.
        doingSetup = true;
        PlayerHealthManager.MakeInvulnerable();
        //Get a reference to our image LevelImage by finding it by name.
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Level " + level;

        //Set levelImage to active blocking player's view of the game board during setup.
        levelImage.SetActive(true);
        SFXManager.PlayMusic();
        //Call the HideLevelImage function with a delay in seconds of levelStartDelay.
        Invoke("HideLevelImage", levelStartDelay);

        boardScript.SetupBoard(level);
    }

    //Hides black image used between levels
    void HideLevelImage()
    {
        //Disable the levelImage gameObject.
        levelImage.SetActive(false);
        PlayerHealthManager.MakeVulnerable(playerStartInvulnerability);
        //Set doingSetup to false allowing player to move again.
        doingSetup = false;
    }

    //Update is called every frame.
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            bool menuOpen = InventoryMenu.ToggleDisplay();
            if (menuOpen) {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void PauseRigidBodies()
    {
        instance.RBs = Object.FindObjectsOfType<PausableRigidBody2D>();
        for (int i = 0; i < instance.RBs.Length; i++)
        {
            instance.RBs[i].Pause();
        }
    }

    public void ResumeRigidBodies()
    {
        for (int i = 0; i < instance.RBs.Length; i++)
        {
            instance.RBs[i].Resume();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        instance.RBs = Object.FindObjectsOfType<PausableRigidBody2D>();
        for (int i = 0; i < instance.RBs.Length; i++)
        {
            instance.RBs[i].Pause();
        }
        SFXManager.Pause();
        instance.defaultDeltaTime = Time.fixedDeltaTime;
        Time.fixedDeltaTime = float.MaxValue;
    }

    public void Resume()
    {
        for (int i = 0; i < instance.RBs.Length; i++)
        {
            instance.RBs[i].Resume();
        }
        SFXManager.Resume();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultDeltaTime;
    }

    public static void MovePlayer(Vector3 destination)
    {
        Debug.Log("player " + destination.x + "," + destination.y);
        Player.instance.gameObject.transform.position = destination;
    }

    public static void GameOver()
    {
        SFXManager.PauseMusic();
        instance.Pause();
        instance.level = 0;
        HUD.GameOver();
    }
}