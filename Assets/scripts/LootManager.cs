using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    CYSTAL_YELLOW,
}

public enum ResourceItemType
{
    GOLD_1, GOLD_3, GOLD_STACK
}

public class LootManager : MonoBehaviour {

    public static LootManager instance = null;
    [System.Serializable]
    public struct ItemPair
    {
        public Item item;
        public ItemType itemType;
    }
    [System.Serializable]
    public struct ResourcePair
    {
        public Resource resource;
        public ResourceItemType resourceType;
    }
    public List<ItemPair> itemPairs;
    public List<ResourcePair> resourcePairs;
    private Dictionary<ItemType, Item> items;
    private Dictionary<ResourceItemType, Resource> resources;

    //Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            items = new Dictionary<ItemType, Item>();
            resources = new Dictionary<ResourceItemType, Resource>();
            foreach (ResourcePair pair in resourcePairs)
            {
                instance.resources.Add(pair.resourceType, pair.resource);
            }

            foreach (ItemPair pair in itemPairs)
            {
                instance.items.Add(pair.itemType, pair.item);
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public static void DropLoot(Enemy enemy, Vector3 position)
    {
        //Instantiate(instance.items[ItemType.CYSTAL_YELLOW], position, Quaternion.Euler(Vector3.zero));
        ResourceItemType drop = ResourceItemType.GOLD_1;
        switch (enemy)
        {
            case Enemy.SLIME_RED:
                drop = ResourceItemType.GOLD_1;
                break;
            case Enemy.KNIGHT_DEFAULT:
                drop = ResourceItemType.GOLD_3;
                break;
            case Enemy.ORC_DEFAULT:
                drop = ResourceItemType.GOLD_STACK;
                break;
            default:
                break;
        }
        Instantiate(instance.resources[drop], position, Quaternion.Euler(Vector3.zero));
    }

}
