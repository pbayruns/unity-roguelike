using UnityEngine;
    
public abstract class Interactable : MonoBehaviour
{

    public string InfoText = "<Press E to interact>";
    public bool interactable = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (interactable && other.tag == "Player")
        {
            HUD.ShowInfoText(InfoText);
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
        if (interactable && other.tag == "Player")
        {
        bool ePressed = Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.E);
            if (ePressed)
            {
                HUD.HideInfoText();
                Interact();
            }
        }
    }

    public abstract void Interact();
}