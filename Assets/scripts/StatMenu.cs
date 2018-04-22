using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatMenu : MonoBehaviour
{

    public static StatMenu instance = null;


    public int Agility;
    public int Strength;
    public int Wisdom;

    public Text StrengthText;
    public Text AgilityText;
    public Text WisdomText;

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
        //DontDestroyOnLoad(gameObject);
    }

    public static void SetStrength(int str)
    {
        instance.StrengthText.text = "" + str;
    }

    public static void SetWidsom(int wis)
    {
        instance.WisdomText.text = "" + wis;
    }

    public static void SetAgility(int agi)
    {
        instance.AgilityText.text = "" + agi;
    }
}
