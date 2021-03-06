﻿using System.Collections;
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

    public HealthDisplay healthDisplay;
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
        //DontDestroyOnLoad(gameObject);
    }


    // Use this for initialization
    void Start()
    {
        RefreshDataDisplay();
    }

    public static void RefreshDataDisplay()
    {
        UpdateHPDisplay(PlayerHealthManager.currentHP, PlayerHealthManager.maxHP);
        UpdateGoldDisplay(ResourceManager.GetGold());
        UpdateLevelDisplay(PlayerStats.currentLevel, PlayerStats.currentExp, PlayerStats.GetXPToNextLvl());
    }
    public static void GameOver()
    {
        RefreshDataDisplay();
        instance.GameOverText.gameObject.SetActive(true);
        instance.GameOverText.text = "YOU";

        SFXManager.PlaySFX(SFX_TYPE.DEATH_EXPLOSION);
        instance.StartCoroutine(instance.GameOverFinish("DIED", 1f));
    }


    public IEnumerator GameOverFinish(string text, float delayTime = 1f)
    {
        yield return WaitForRealSeconds(delayTime);
        instance.GameOverText.text = text;
        SFXManager.PlaySFX(SFX_TYPE.DEATH_EXPLOSION);
        GameManager.instance.Resume(false, false);
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
        GameManager.instance.StartCoroutine(HideInfoTextTimed(1f));
        //instance.Invoke("HideInfoText", time);
    }

    public static IEnumerator HideInfoTextTimed(float delayTime = 1f)
    {
        yield return new WaitForSeconds(delayTime);
        if (instance != null)
        {
            instance.InfoText.text = "";
            instance.InfoText.gameObject.SetActive(false);
        }
    }

    public static void ShowInfoText(string text)
    {
        instance.InfoText.gameObject.SetActive(true);
        instance.InfoText.text = text;
    }

    public static void HideInfoText()
    {
        if (instance != null)
        {
            instance.InfoText.text = "";
            instance.InfoText.gameObject.SetActive(false);
        }
    }

    public static void UpdateHPDisplay(int current, int max)
    {
        if (instance == null) return;
        instance.healthDisplay.UpdateHP(current, max);
    }

    public static void UpdateLevelDisplay(int level, int xp, int nextLvlXp = -1)
    {
        if (instance == null) return;
        instance.LevelText.text = "LVL " + level + " (Next: " + nextLvlXp + "XP)";
    }

    public static void UpdateGoldDisplay(int current)
    {
        instance.GoldText.text = "" + current;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
