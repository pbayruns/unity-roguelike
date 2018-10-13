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

    public static Item selectedItem = null;

    public static int GetInventoryLimit()
    {
        Debug.Log(instance.item_limit);
        return instance.item_limit;
    }

    public static void ItemSelected(Item theItem){
        selectedItem = theItem;
        OnChange();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ListUtil.Resize(instance.inventory, instance.item_limit, null);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public static bool IsFull()
    {
        for(int i = 0; i < instance.inventory.Count; i++){
            if(instance.inventory[i] == null){
                return false;
            }
        }
        return true;
    }
    
    public static bool swap(int fromNdx, int toNdx)
    {
        // Debug.Log("moving from " + fromNdx);
        // Debug.Log("to " + toNdx);
        // Debug.Log(instance.inventory.Count);

        Item temp = instance.inventory[toNdx];
        instance.inventory[toNdx] = instance.inventory[fromNdx];
        instance.inventory[fromNdx] = temp;
        OnChange();
        return true;
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
        // Debug.Log("removed" + removed);
        // Debug.Log("current" + current);
        AddItem(current, index);
        //equipped[slot] = removed;
        equipped[slot] = removed;
        OnChange();
        return true;
    }

    public static bool AddItem(Item item, int index = -1)
    {
        if(IsFull()){
            return false;
        }
        if(index == -1){
            for(int i = 0; i < instance.item_limit; i++){
                if(instance.inventory[i] == null){
                    instance.inventory[i] = item;
                    OnChange();
                    return true;
                }
            }
            return false;
        } else {
            instance.inventory[index] = item;
            OnChange();
            return true;
        }
    }

    public static bool Remove(Item item)
    {
        int ndx = instance.inventory.IndexOf(item);
        return Remove(ndx);
    }

    public static Item Remove(int index)
    {
        if(index == -1){
            OnChange();
            return null;
        } else {
            Item removed = instance.inventory[index];
            instance.inventory[index] = null;
            OnChange();
            return removed;
        }
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
