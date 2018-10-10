using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler//, IDropHandler
{
    public Item item;
    public InventorySlot slot;

    Vector3 startPosition;
    Transform startParent;
    Transform canvas;

    // DRAG
    public void OnBeginDrag(PointerEventData data)
    {
        slot = GetComponentInParent<InventorySlot>();
        item = slot.GetItem();

        startPosition = transform.position;
        startParent = transform.parent;
        canvas = GameObject.FindGameObjectWithTag("UI").transform;
        gameObject.transform.SetParent(canvas, false);
    }

    public void OnDrag(PointerEventData data)
    {
        transform.position = data.position;
    }

    // DROP
    public void OnEndDrag(PointerEventData data)
    {
        if (transform.parent == canvas)
        {
            transform.SetParent(startParent, false);
            transform.position = startPosition;
        }

        RectTransform invPanel = InventoryMenu.instance.inventoryUI.transform as RectTransform;
        RectTransform equipPanel = InventoryMenu.instance.equipUI.transform as RectTransform;

        bool onInv = RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition);
        bool onEquip = RectTransformUtility.RectangleContainsScreenPoint(equipPanel, Input.mousePosition);
        if (!onInv && !onEquip)
        {
            //drop is outside inv and equip
            // return item back to where it was
            Debug.Log("not on anything");
            transform.SetParent(startParent, false);
            transform.position = startPosition;
        }
        else if (onInv)
        {
            // if it's on the inventory, we want to find what slot it was dropped into
            RectTransform invDropSlot;
            for(int i = 0; i < InventoryMenu.instance.slots.Length; i++){
                InventorySlot droppedSlot = InventoryMenu.instance.slots[i];
                invDropSlot = droppedSlot.transform as RectTransform;
                if(RectTransformUtility.RectangleContainsScreenPoint(invDropSlot, Input.mousePosition)){
                    int fromNdx = slot.transform.GetSiblingIndex();
                    int toNdx = droppedSlot.transform.GetSiblingIndex();
                    // swap the contents of slot and dropped slot
                    InventoryManager.swap(fromNdx, toNdx);
                }
            }
        }
        else if (onEquip)
        {
            // If it's on equip, we want to try to equip it in the given slot
            Debug.Log("item quipslot " + item.EquipSlot);
            InventoryManager.Equip(item, item.EquipSlot);
        }
    }
}