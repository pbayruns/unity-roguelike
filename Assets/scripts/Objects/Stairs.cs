using UnityEngine;
    
public class Stairs : Interactable
{

    public Stairs(){
        this.InfoText = "<Press E to descend>";
    }

    public override void Interact(){
        SFXManager.PlaySFX(SFX_TYPE.STAIRS_DOWN);
        Player.Restart();
    }
}