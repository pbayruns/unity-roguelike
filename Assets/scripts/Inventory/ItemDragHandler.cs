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
            transform.position = startPosition;
            transform.SetParent(startParent, false);
        }

        RectTransform invPanel = InventoryMenu.instance.inventoryUI.transform as RectTransform;
        RectTransform equipPanel = InventoryMenu.instance.equipUI.transform as RectTransform;

        bool onInv = RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition);
        bool onEquip = RectTransformUtility.RectangleContainsScreenPoint(equipPanel, Input.mousePosition);
        if (!onInv && !onEquip)
        {
            //drop is outside inv and equip
        }
        else if (onInv)
        {
        }
        else if (onEquip)
        {
            Debug.Log("item quipslot " + item.EquipSlot);
            InventoryManager.Equip(item, item.EquipSlot);
        }
        else
        {

        }
    }
}