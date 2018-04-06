using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    public static int item_limit = 10;
    public static List<Item> inventory = new List<Item>();
    public enum EquipSlot {
        HEAD, BODY, LEGS, ARMS,
        RING_1, RING_2, WEAPON,
        NONE
    };
    public static Dictionary<EquipSlot, Item> equipped;
    
    public static bool EquipItem(Item item, EquipSlot slot)
    {
        int index = inventory.IndexOf(item);
        if(index >= 0)
        {
            return EquipItem(index, slot);
        }
        return false;
    }

    public static bool EquipItem(int index, EquipSlot slot)
    {
        // get current equipped item
        // get new item
        // remove equipped item from equipped
        // remove new item from inventory
        // add equipped to inventory
        // add item to equipped
        Item current = equipped[slot];
        Item removed = RemoveItemFromInventory(index);
        inventory[index] = current;
        equipped[slot] = removed;
        return true;
    }

    public static bool AddItemToInventory(Item item, int index = -1)
    {
        if(inventory.Count != item_limit)
        {
            if(index >= 0)  inventory.Insert(index, item);
            else inventory.Add(item);
            return true;
        }
        return false;
    }

    public static bool RemoveItemFromInventory(Item item)
    {
        return inventory.Remove(item);
    }

    public static Item RemoveItemFromInventory(int index)
    {
        if(index >= 0 && index < inventory.Count)
        {
            Item removed = inventory[index];
            inventory.RemoveAt(index);
            return removed;
        }
        return null;
    }

    public static void DisplayInventory()
    {
        InventoryMenu.ShowDisplay();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
