using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPotion : Item {
    
    public RedPotion(){

    }

    public override string GetInfo(){
        return "A potion that restores 5 hearts.";
    }

    public override string GetDisplayName(){
        return "Red Potion";
    }

    public override void Use()
    {
        PlayerHealthManager.HealPlayer(20);
        InventoryManager.Remove(this);
        InventoryManager.ItemSelected(null);
    }

    public override bool canUse(){
        return true;
    }
}
