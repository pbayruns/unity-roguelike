using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour {


    public InventorySlot item = null;
    public Text infoText;
    public Text titleText;
    public Button UseButton;

    const string DEFAULT_INFO_TEXT = "Click an item and its info will be displayed here.";
    const string DEFAULT_TITLE_TEXT = "";

    public void updateItem(Item newItem){
        item.AddItem(newItem);
        if(newItem != null){
            infoText.text = newItem.GetInfo();
            titleText.text = newItem.GetDisplayName();
            if(newItem.canUse()){
                UseButton.gameObject.SetActive(true);
                UseButton.onClick.AddListener(newItem.Use);
            } else {
                UseButton.gameObject.SetActive(false);
            }
        } else {
            infoText.text = DEFAULT_INFO_TEXT;
            titleText.text = DEFAULT_TITLE_TEXT;
            UseButton.gameObject.SetActive(false);
        }
    }
    
    public void Start(){
        UseButton.gameObject.SetActive(false);
    }
    public void Use()
    {
        item.UseItem();
    }


}
