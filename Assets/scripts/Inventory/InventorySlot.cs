using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{

    public Image icon;          // Reference to the Icon image
    public Image panelImage;
    public ItemDragHandler handler;
    public bool selectable = true;
    Item item;  // Current item in the slot
    Color originalColor;

    // Add item to the slot
    public void AddItem(Item newItem)
    {

        // GameObject child1 = transform.GetChild(0).gameObject;
        // ItemDragHandler handler = child1.GetComponent<ItemDragHandler>();
        if (handler != null)
        {
            handler.item = newItem;
            handler.slot = this;
        }
        if (newItem == null)
        {
            ClearSlot();
            return;
        }
        else
        {
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

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            Debug.Log("selected " + selected);
            // make it green
            panelImage.color = Colorutil.GetColor(19, 234, 2);
        }
        else
        {
            panelImage.color = Color.white;
        }
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        if (selectable)
        {
            InventoryManager.ItemSelected(this.item);
            SetSelected(true);
        }
    }
}