using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour {

    public static InventoryMenu instance = null;

    public Transform itemsParent;   // The parent object of all the items
    public GameObject inventoryUI;  // The entire UI
    public InventorySlot slotPrefab;
    public GameObject equipUI;
    public GameObject infoUI;
    public ItemInfo itemInfo;

    public Transform equipParent;

    public InventorySlot armor;
    public InventorySlot weapon;
    public InventorySlot ring;

    InventoryManager inventory;    // Our current inventory

    public InventorySlot[] slots;  // List of all the slots
    InventorySlot[] equipment;
    InventorySlot selectedItem; // currently selected item

    //Awake is always called before any Start functions
    public void Awake()
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
        //DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        inventory = InventoryManager.instance;
        inventory.onItemChangedCallback = UpdateUI;    // Subscribe to the onItemChanged callback
        for (int i = 0; i < InventoryManager.GetInventoryLimit(); i++) {
            InventorySlot slot = Instantiate(slotPrefab);
            slot.transform.SetParent(itemsParent);
        }
        // Populate our slots array
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update the inventory UI by:
    //		- Adding items
    //		- Clearing empty slots
    // This is called using a delegate on the Inventory.
    public void UpdateUI()
    {
        instance.itemInfo.updateItem(InventoryManager.selectedItem);
        // Loop through all the slots
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.inventory.Count)  // If there is an item to add
            {
                slots[i].AddItem(inventory.inventory[i]);   // Add it
                slots[i].SetSelected(false);
            }
            else
            {
                // Otherwise clear the slot
                slots[i].ClearSlot();
            }
            if(slots[i].GetItem() != null && slots[i].GetItem() == InventoryManager.selectedItem){
                slots[i].SetSelected(true);
            }
        }

        if(InventoryManager.equipped.ContainsKey(EquipSlot.ARMOR) &&
            InventoryManager.equipped[EquipSlot.ARMOR] != null)
        {
            armor.AddItem(InventoryManager.equipped[EquipSlot.ARMOR]);
        }
        else
        {
            armor.ClearSlot();
        }

        if (InventoryManager.equipped.ContainsKey(EquipSlot.WEAPON) &&
            InventoryManager.equipped[EquipSlot.WEAPON] != null)
        {
            weapon.AddItem(InventoryManager.equipped[EquipSlot.WEAPON]);
        }
        else
        {
            weapon.ClearSlot();
        }
    }

    public static bool ToggleDisplay()
    {
        bool state = instance.inventoryUI.gameObject.activeSelf;
        bool menuOpen = !state;
        instance.inventoryUI.gameObject.SetActive(menuOpen);
        instance.equipUI.gameObject.SetActive(menuOpen);
        instance.infoUI.gameObject.SetActive(menuOpen);
        if(menuOpen){
            instance.UpdateUI();
        }
        InventoryManager.ItemSelected(null);
        return menuOpen;
    }

    public static void Close()
    {
        instance.inventoryUI.gameObject.SetActive(false);
    }
}
