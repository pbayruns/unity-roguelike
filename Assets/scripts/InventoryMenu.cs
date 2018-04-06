using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour {

    public static InventoryMenu instance = null;

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

    public static void ShowDisplay()
    {
        instance.gameObject.SetActive(true);
    }

    public static void HideDisplay()
    {
        instance.gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
