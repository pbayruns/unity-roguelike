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

    public Text PointsAvaiableText;
    private const string pointsString = "Stat Points Available: ";

    public Button PlusStrength;
    public Button PlusAgility;
    public Button PlusWisdom;

    public GameObject statUI;  // The entire UI

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

    private void Update()
    {
        PointsAvaiableText.text = pointsString + PlayerStats.availableStatpoints;
        if (PlayerStats.availableStatpoints <= 0)
        {
            PlusStrength.gameObject.SetActive(false);
            PlusAgility.gameObject.SetActive(false);
            PlusWisdom.gameObject.SetActive(false);
        }
        else
        {
            PlusStrength.gameObject.SetActive(true);
            PlusAgility.gameObject.SetActive(true);
            PlusWisdom.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        PlusStrength.onClick.AddListener(AddStrength);
        PlusAgility.onClick.AddListener(AddAgility);
        PlusWisdom.onClick.AddListener(AddWisdom);
    }

    public static void AddStrength()
    {
        PlayerStats.AddStrength(1);
        PlayerStats.availableStatpoints -= 1;
        SetStrength(PlayerStats.strength);
    }

    public static void AddAgility()
    {
        PlayerStats.AddAgility(1);
        PlayerStats.availableStatpoints -= 1;
        SetAgility(PlayerStats.agility);
    }

    public static void AddWisdom()
    {
        PlayerStats.AddWisdom(1);
        PlayerStats.availableStatpoints -= 1;
        SetWisdom(PlayerStats.wisdom);
    }

    public static void SetStrength(int str)
    {
        instance.StrengthText.text = "" + str;
    }

    public static void SetWisdom(int wis)
    {
        instance.WisdomText.text = "" + wis;
    }

    public static void SetAgility(int agi)
    {
        instance.AgilityText.text = "" + agi;
    }

    public static bool ToggleDisplay()
    {
        SetWisdom(PlayerStats.wisdom);
        SetStrength(PlayerStats.strength);
        SetAgility(PlayerStats.agility);

        bool state = instance.statUI.gameObject.activeSelf;
        bool menuOpen = !state;
        instance.statUI.gameObject.SetActive(menuOpen);
        return menuOpen;
    }

    public static void Close()
    {
        instance.statUI.gameObject.SetActive(false);
    }
}
