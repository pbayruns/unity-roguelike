using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour {

    public static LootManager instance = null;
    public Item CRYSTAL_YELLOW;

    //Singleton
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

    public static void DropLoot(Enemy enemy, Vector3 position)
    {
        Debug.Log("dropping loot");
        Instantiate(instance.CRYSTAL_YELLOW, position, Quaternion.Euler(Vector3.zero));
    }

}
