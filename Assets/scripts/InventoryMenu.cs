using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour {

    public static InventoryMenu instance = null;
    public Image menuPanel;
    
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
    }

    public static bool ToggleDisplay()
    {
        bool state = instance.menuPanel.gameObject.activeSelf;
        bool menuOpen = !state;
        instance.menuPanel.gameObject.SetActive(menuOpen);
        return menuOpen;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
