using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public Sprite icon;
    public EquipSlot EquipSlot;
    public int attack = 0;
    public int defense = 0;
    public int strength = 0;
    public int agility = 0;
    public int wisdom = 0;
    
    public string GetInfo(){
        return "This is the item description.";
    }

    public string GetDisplayName(){
        return "Item Name Here";
    }

    public void Use()
    {

    }
}
