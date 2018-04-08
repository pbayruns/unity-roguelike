using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupResource : MonoBehaviour
{

    public Resource resource;
    private ResourceType resourceType;

    // Use this for initialization
    void Start()
    {
        resource = GetComponent<Resource>();
        resourceType = resource.resourceType;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!ResourceManager.IsFull(resourceType))
            {
                ResourceManager.AddResource(resource);
                Destroy(gameObject);
            }
            else
            {
                HUD.ShowInfoTextTimed("Resource Full", 1f);
            }
        }
    }
}
