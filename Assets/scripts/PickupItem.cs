﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {

    public Item item;

	// Use this for initialization
	void Start () {
        item = GetComponent<Item>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (InventoryManager.AddItem(item))
            {
                Debug.Log("added to inventory");
                HUD.ShowInfoTextTimed("Item added to inventory", 1f);
                Destroy(gameObject);
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