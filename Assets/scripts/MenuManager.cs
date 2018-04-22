using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Menu
{
    STATS, INVENTORY
}
public class MenuManager : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}

    public static void Toggle(Menu menu)
    {
        bool menuOpen = false;
        switch (menu)
        {
            case Menu.INVENTORY:
                menuOpen = InventoryMenu.ToggleDisplay();
                StatMenu.Close();
                break;
            case Menu.STATS:
                menuOpen = StatMenu.ToggleDisplay();
                InventoryMenu.Close();
                break;
        }
        if (menuOpen && !GameManager.Paused)
        {
            GameManager.instance.Pause();
        }
        else if(!menuOpen && GameManager.Paused)
        {
            GameManager.instance.Resume();
        }
    }
}
