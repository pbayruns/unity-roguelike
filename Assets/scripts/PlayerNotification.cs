using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNotification : MonoBehaviour {

    public static PlayerNotification instance = null;
    public float moveSpeed = 5;
    public GameObject display;
    public static Color SUCCESS_COLOR = new Color(64f, 189f, 91f, 1f);

    // Keep only one instance at all times
    private void Awake()
    {
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

    private static void DisplayNotification(string message, Color color, float time = 1f)
    {
        var clone = (GameObject)Instantiate(instance.display, Player.GetPosition(), Quaternion.Euler(Vector3.zero));
        NotificationText notif = clone.GetComponentInChildren<NotificationText>();
        notif.SetText(message);
        notif.SetColor(SUCCESS_COLOR);
        notif.SetTime(time);
        notif.SetSpeed(instance.moveSpeed);
    }

    public static void DisplayXPNotification(int xpAdded)
    {
        DisplayNotification(xpAdded + "XP", SUCCESS_COLOR);
    }

    public static void DisplayLevelUpNotification(int level, int attackAdded, int defenseAdded, int hpAdded)
    {
        string message = "Level Up: LVL " + level + "!" + "\n";
        if (attackAdded > 0) message += attackAdded + "STR" + "\n";
        if (defenseAdded > 0) message += defenseAdded + "DEF" + "\n";
        if (hpAdded > 0) message += hpAdded + "HP" + "\n";

        DisplayNotification(message, SUCCESS_COLOR, 5f);
    }
}
