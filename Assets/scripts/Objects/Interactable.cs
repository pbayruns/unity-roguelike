using UnityEngine;
    
public abstract class Interactable : MonoBehaviour
{

    public string InfoText = "<Press E to interact>";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            HUD.ShowInfoTextTimed(InfoText, 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            HUD.HideInfoText();
        }
    }

        private void OnTriggerStay2D(Collider2D other)
    {
        bool ePressed = Input.GetKeyDown(KeyCode.E);
        if (other.tag == "Stairs")
        {
            HUD.ShowInfoText(InfoText);
            if (ePressed)
            {
                HUD.HideInfoText();
                Interact();
            }
        }
    }

    public abstract void Interact();
}