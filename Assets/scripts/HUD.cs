using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public static HUD instance = null;  //Static instance of Player which allows it to be accessed by any other script.
    public Text InfoText;
    public Text HPText;
    public Text GoldText;
    public Text LevelText;
    public Text GameOverText;

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
    }

    public static void GameOver()
    {
        instance.GameOverText.gameObject.SetActive(true);
        instance.GameOverText.text = "YOU";
        SFXManager.PlaySFX(SFX_TYPE.DEATH_EXPLOSION);
        instance.StartCoroutine(instance.GameOverFinish("YOU DIED", 1f));
    }


    public IEnumerator GameOverFinish(string text, float delayTime = 1f)
    {
        yield return WaitForRealSeconds(delayTime);
        instance.GameOverText.text = text;
        SFXManager.PlaySFX(SFX_TYPE.DEATH_EXPLOSION);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        GameManager.instance.Resume();
    }

    public IEnumerator _WaitForRealSeconds(float aTime)
    {
        while (aTime > 0f)
        {
            aTime -= Mathf.Clamp(Time.unscaledDeltaTime, 0, 0.2f);
            yield return null;
        }
    }
    public Coroutine WaitForRealSeconds(float aTime)
    {
        return StartCoroutine(_WaitForRealSeconds(aTime));
    }

    //TODO: make this work later
    public static void ShowInfoTextTimed(string text, float time = 1f)
    {
        ShowInfoText(text);
        //instance.Invoke("HideInfoText", time);
    }

    public static void ShowInfoText(string text)
    {
        instance.InfoText.gameObject.SetActive(true);
        instance.InfoText.text = text;
    }

    public static void HideInfoText()
    {
        if(instance != null)
        {
            instance.InfoText.text = "";
            instance.InfoText.gameObject.SetActive(false);
        }
    }

    public static void UpdateHPDisplay(int current, int max)
    {
        instance.HPText.text = "HP: " + current + "+" + max;
    }

    public static void UpdateLevelDisplay(int level, int xp)
    {
        instance.LevelText.text = "LVL " + level + " (" + xp + "XP)";
    }

    public static void UpdateGoldDisplay(int current)
    {
        instance.GoldText.text = "Gold: " + current;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
