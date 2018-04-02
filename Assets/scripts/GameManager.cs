using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;	//Time to wait before starting level, in seconds.
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;
    public bool doingSetup;
    public CameraController cam;

    private Text levelText;                                 //Text to display current level number.
    private GameObject levelImage;                          //Image to block out level as levels are being set up, background for levelText.

    private int level = 1; //Current level number
    //private bool doingSetup = true; //bool used to prevent Player from moving during setup.	
    private BoardCreator boardScript; //BoardManager which will set up the level.

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

        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardCreator>();

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
        //While doingSetup is true the player can't move, prevent player from moving while title card is up.
        doingSetup = true;

        //Call the SetupScene function of the BoardManager script, pass it current level number.
        //boardScript.SetupScene(level);
        //Instantiate(boardScript);

        //Get a reference to our image LevelImage by finding it by name.
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Level " + level;

        //Set levelImage to active blocking player's view of the game board during setup.
        levelImage.SetActive(true);

        //Call the HideLevelImage function with a delay in seconds of levelStartDelay.
        Invoke("HideLevelImage", levelStartDelay);
    }

    //Hides black image used between levels
    void HideLevelImage()
    {
        //Disable the levelImage gameObject.
        levelImage.SetActive(false);

        //Set doingSetup to false allowing player to move again.
        doingSetup = false;
    }

    //GameOver is called when the player reaches 0 food points
    public void GameOver()
    {
        //Disable this GameManager.
        enabled = false;
    }

    //Update is called every frame.
    void Update()
    {

    }
}