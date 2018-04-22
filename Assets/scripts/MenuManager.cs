using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Menu
{
    STATS, INVENTORY, SETTINGS
}
public class MenuManager : MonoBehaviour {

    public static void Toggle(Menu menu)
    {
        bool menuOpen = false;
        switch (menu)
        {
            case Menu.INVENTORY:
                menuOpen = InventoryMenu.ToggleDisplay();
                CloseMenusExcept(Menu.INVENTORY);
                break;
            case Menu.STATS:
                menuOpen = StatMenu.ToggleDisplay();
                CloseMenusExcept(Menu.STATS);
                break;
            case Menu.SETTINGS:
                menuOpen = SettingsMenu.ToggleDisplay();
                CloseMenusExcept(Menu.SETTINGS);
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

    static void CloseMenusExcept(Menu dontClose)
    {
        if (dontClose != Menu.INVENTORY) InventoryMenu.Close();
        if (dontClose != Menu.SETTINGS) SettingsMenu.Close();
        if (dontClose != Menu.STATS) StatMenu.Close();
    }
}
