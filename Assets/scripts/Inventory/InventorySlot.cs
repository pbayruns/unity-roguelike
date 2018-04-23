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
        if (newItem == null) { ClearSlot(); return; }
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        icon.gameObject.SetActive(true);
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