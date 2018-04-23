using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler//, IDropHandler
{
    public Item item;
    public InventorySlot slot;

    // DRAG
    public void OnBeginDrag(PointerEventData eventData)
    {
        slot = GetComponentInParent<InventorySlot>();
        item = slot.GetItem();
    }

    public void OnDrag(PointerEventData data)
    {
        transform.position = Input.mousePosition;
    }

    // DROP
    public void OnEndDrag(PointerEventData data)
    {
        RectTransform invPanel = InventoryMenu.instance.inventoryUI.transform as RectTransform;
        RectTransform equipPanel = InventoryMenu.instance.equipUI.transform as RectTransform;

        bool onInv = RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition);
        bool onEquip = RectTransformUtility.RectangleContainsScreenPoint(equipPanel, Input.mousePosition);
        if (!onInv && !onEquip)
        {
            //drop is outside inv and equip
            transform.localPosition = Vector3.zero;
        }
        else if (onInv)
        {
            transform.localPosition = Vector3.zero;
        }
        else if (onEquip)
        {
            slot = GetComponentInParent<InventorySlot>();
            item = slot.GetItem();
            Debug.Log("item quipslot " + item.EquipSlot);
            InventoryManager.Equip(item, item.EquipSlot);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }
}