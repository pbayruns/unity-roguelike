using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {

    public Item item;

	// Use this for initialization
	void Start () {
        item = GetComponent<Item>();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
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
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
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
}
