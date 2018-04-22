using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;	//Time to wait before starting level, in seconds.
    public float playerStartInvulnerability = 2f;
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;
    public Player player;
    private Text levelText;                                 //Text to display current level number.
    private GameObject levelImage;                          //Image to block out level as levels are being set up, background for levelText.
    private float defaultDeltaTime;
    public int level = 0; //Current level number

    public BoardCreator boardScript; //BoardManager which will set up the level.
    private PausableRigidBody2D[] RBs;

    //Awake is always called before any Start functions
    private static bool _paused = false;
    private static bool _enemiespaused = true;

    public static bool Paused
    {
        get
        {
            return _paused;
        }
        set
        {
            _paused = value;
        }
    }

    public static bool EnemiesPaused
    {
        get
        {
            return _enemiespaused;
        }
        set
        {
            _enemiespaused = value;
        }
    }

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
        player = FindObjectOfType<Player>();
       
        //Call the InitGame function to initialize the first level 
        //InitGame();
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
    static private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name == "main") {
            instance.level++;
            instance.InitGame();
        }
    }

    //Initializes the game for each level.
    void InitGame()
    {
        _enemiespaused = true;
        _paused = true;

        boardScript = FindObjectOfType<BoardCreator>();

        //While doingSetup is true the player can't move, prevent player from moving while title card is up.
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

        _enemiespaused = false;
        _paused = false;
    }

    //Update is called every frame.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            MenuManager.Toggle(Menu.INVENTORY);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            MenuManager.Toggle(Menu.STATS);
        }
    }

    //public void PauseRigidBodies()
    //{
    //    instance.RBs = Object.FindObjectsOfType<PausableRigidBody2D>();
    //    for (int i = 0; i < instance.RBs.Length; i++)
    //    {
    //        instance.RBs[i].Pause();
    //    }
    //}

    //public void ResumeRigidBodies()
    //{
    //    for (int i = 0; i < instance.RBs.Length; i++)
    //    {
    //        instance.RBs[i].Resume();
    //    }
    //}

    public void Pause()
    {
        _paused = true;
        _enemiespaused = true;
        Time.timeScale = 0f;
        //instance.RBs = Object.FindObjectsOfType<PausableRigidBody2D>();
        //for (int i = 0; i < instance.RBs.Length; i++)
        //{
        //    instance.RBs[i].Pause();
        //}
        SFXManager.Pause();
        instance.defaultDeltaTime = Time.fixedDeltaTime;
        Time.fixedDeltaTime = float.MaxValue;
    }

    public void Resume(bool withMusic = true, bool withEnemies = true)
    {
        _paused = false;
        Debug.Log("with enemies" + withEnemies);
        if (withEnemies)
        {
            _enemiespaused = false;
            //for (int i = 0; i < instance.RBs.Length; i++)
            //{
            //    instance.RBs[i].Resume();
            //}
        }
        if (withMusic) SFXManager.Resume();
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
        PlayerStats.Reset();
        PlayerHealthManager.Reset();
        ResourceManager.Reset();
        HUD.GameOver();
        instance.Invoke("InstanceGoToMenu", 2f);
    }

    private void InstanceGoToMenu()
    {
        _paused = false;
        GoToMenu();
    }

    public static void GoToMenu()
    {
        SceneManager.LoadScene("menu", LoadSceneMode.Single);
        DestroyOnMenuObjects();
    }

    public static void DestroyOnMenuObjects()
    {
        DestroyOnMenu[] notInMenu = GameObject.FindObjectsOfType<DestroyOnMenu>();
        for (int i = 0; i < notInMenu.Length; i++)
        {
            Destroy(notInMenu[i].gameObject);
        }
    }
}