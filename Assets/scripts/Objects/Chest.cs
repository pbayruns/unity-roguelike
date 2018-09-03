using UnityEngine;
    
public class Chest : Interactable
{
    public Sprite CHEST_OPEN;

    public Chest(){
        this.InfoText = "<Press E to open>";
    }

    public override void Interact(){
        this.GetComponent<SpriteRenderer>().sprite = CHEST_OPEN;    
        SFXManager.PlaySFX(SFX_TYPE.STAIRS_DOWN);
    }
}