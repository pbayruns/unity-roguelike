using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipSlot
{
    ARMOR,
    RING_1, RING_2, WEAPON,
    NONE
};
public class InventoryManager : MonoBehaviour
{

    public static InventoryManager instance = null;
    // Callback which is triggered when
    // an item gets added/removed.
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int item_limit = 16;
    public List<Item> inventory = new List<Item>();

    public static Dictionary<EquipSlot, Item> equipped = new Dictionary<EquipSlot, Item>();

    public static int GetInventoryLimit()
    {
        return instance.item_limit;
    }

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

    public static bool IsFull()
    {
        return instance.item_limit == instance.inventory.Count;
    }

    public static bool Equip(Item item, EquipSlot slot)
    {
        int index = instance.inventory.IndexOf(item);
        if (index >= 0)
        {
            return Equip(index, slot);
        }
        return false;
    }

    public static bool Equip(int index, EquipSlot slot)
    {
        // get current equipped item
        // get new item
        // remove equipped item from equipped
        // remove new item from inventory
        // add equipped to inventory
        // add item to equipped
        Item current = null;
        equipped.TryGetValue(slot, out current);
        Item removed = Remove(index);
        Debug.Log("removed" + removed);
        Debug.Log("current" + current);
        AddItem(current, index);
        //equipped[slot] = removed;
        equipped[slot] = removed;
        OnChange();
        return true;
    }

    public static bool AddItem(Item item, int index = -1)
    {
        if (instance.inventory.Count < instance.item_limit)
        {
            if (index >= 0) instance.inventory.Insert(index, item);
            else instance.inventory.Add(item);
            OnChange();
            return true;
        }
        return false;
    }

    public static bool Remove(Item item)
    {
        OnChange();
        return instance.inventory.Remove(item);
    }

    public static Item Remove(int index)
    {
        if (index >= 0 && index < instance.inventory.Count)
        {
            Item removed = instance.inventory[index];
            instance.inventory.RemoveAt(index);
            OnChange();
            return removed;
        }
        return null;
    }

    private static void OnChange()
    {
        // Trigger callback
        if (instance.onItemChangedCallback != null)
        {
            instance.onItemChangedCallback.Invoke();
        }
    }
}
