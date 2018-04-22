using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    public static SettingsMenu instance = null;

    public Button QuitToMenu;

    public GameObject settingsUI;  // The entire UI

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

    private void Start()
    { 
        QuitToMenu.onClick.AddListener(GameManager.GoToMenu);
    }

    public static bool ToggleDisplay()
    {
        bool state = instance.settingsUI.gameObject.activeSelf;
        bool menuOpen = !state;
        instance.settingsUI.gameObject.SetActive(menuOpen);
        return menuOpen;
    }

    public static void Close()
    {
        instance.settingsUI.gameObject.SetActive(false);
    }
}
