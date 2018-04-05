﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public static HUD instance = null;  //Static instance of Player which allows it to be accessed by any other script.
    public Text InfoText;

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

    public static void ShowInfoText(string text)
    {
        instance.InfoText.gameObject.SetActive(true);
        instance.InfoText.text = text;
    }

    public static void HideInfoText()
    {
        instance.InfoText.text = "";
        instance.InfoText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
