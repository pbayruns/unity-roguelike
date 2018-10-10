using UnityEngine;
using UnityEngine.UI;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour
{

    public Image icon;          // Reference to the Icon image

    Item item;  // Current item in the slot
    // Add item to the slot
    public void AddItem(Item newItem)
    {

        GameObject child1 = transform.GetChild(0).gameObject;
        ItemDragHandler handler = child1.GetComponent<ItemDragHandler>();
        if(handler != null){
            handler.item = newItem;
            handler.slot = this;
        }
        if (newItem == null) { 
            ClearSlot(); 
            return; 
        } else {
            item = newItem;
            icon.sprite = item.icon;
            icon.enabled = true;
            icon.gameObject.SetActive(true);
        }

    }

    public Item GetItem()
    {
        return item;
    }

    // Clear the slot
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    // Called when the item is pressed
    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

}