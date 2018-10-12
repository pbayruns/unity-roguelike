using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {

    public Item item;

    public bool _grabbable = true;    
    public bool Grabbable {
        get { return _grabbable; }
        set { _grabbable = value; }
    }

	// Use this for initialization
	void Start () {
        item = GetComponent<Item>();
	}

    public void Float(){
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sortingLayerName = "Top";
    }

    public void Ground(){
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sortingLayerName = "Items";
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_grabbable && other.gameObject.tag == "Player")
        {
            Pickup();
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (_grabbable && other.gameObject.tag == "Player")
        {
            if (!InventoryManager.IsFull())
            {
                HUD.ShowInfoText("<Press E to pick up>");
            }
            else
            {
                HUD.ShowInfoText("Inventory full");
            }
        }
    }

    public bool Pickup(){
        if (InventoryManager.AddItem(item))
        {
            HUD.ShowInfoTextTimed("Item added to inventory", 1f);
            //Destroy(gameObject);
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(GetComponent<DestroyOnMenu>());
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            Destroy(this);
            return true;
        } 
        return false;
    }
}
