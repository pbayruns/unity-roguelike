using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    NONE,
    CYSTAL_YELLOW, 
    SWORD_COPPER
}

public enum ResourceItemType
{
    NONE,
    GOLD_1, GOLD_3, GOLD_STACK
}

public class LootManager : MonoBehaviour {

    public static LootManager instance = null;
    private Dictionary<ItemType, Object> ItemPool = new Dictionary<ItemType, Object>();
    private Dictionary<ResourceItemType, Object> ResourcePool = new Dictionary<ResourceItemType, Object>();

    //Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public static void DropLoot(Enemy enemy, Vector3 position)
    {
        InstantiateItem(ItemType.SWORD_COPPER, position);

        switch (enemy)
        {
            case Enemy.SLIME_RED:
                InstantiateResource(ResourceItemType.GOLD_1, position);
                return;
            case Enemy.KNIGHT_DEFAULT:
                InstantiateResource(ResourceItemType.GOLD_3, position);
                return;
            case Enemy.ORC_DEFAULT:
                InstantiateResource(ResourceItemType.GOLD_STACK, position);
                return;
            default:
                return;
        }
    }


    public static void InstantiateItem(ItemType item, Vector3 position)
    {
        if (item == ItemType.NONE) return;

        Object itemObject;
        // try to load the gameobject from our cache, if it doesn't load, load it from resources and cache it.
        if (instance.ItemPool.ContainsKey(item))
        {
            itemObject = instance.ItemPool[item];
        }
        else
        {
            string itemName = System.Enum.GetName(typeof(ItemType), item);
            itemObject = Resources.Load("Items/" + itemName);
            instance.ItemPool[item] = itemObject;
        }

        // Spawn the item
        Instantiate(itemObject, position, Quaternion.identity);
    }

    public static void InstantiateResource(ResourceItemType resource, Vector3 position)
    {
        if (resource == ResourceItemType.NONE) return;

        Object resourceObject;
        // try to load the gameobject from our cache, if it doesn't load, load it from resources and cache it.
        if (instance.ResourcePool.ContainsKey(resource))
        {
            resourceObject = instance.ResourcePool[resource];
        }
        else
        {
            string resourceName = System.Enum.GetName(typeof(ResourceItemType), resource);
            resourceObject = Resources.Load("Stackables/" + resourceName);
            instance.ResourcePool[resource] = resourceObject;
        }

        // Spawn the resource
        Instantiate(resourceObject, position, Quaternion.identity);
    }

}
