using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public int gold = 0;
    public static ResourceManager instance = null;

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

    public static bool IsFull(ResourceType type)
    {
        //Put checks for resource limits here (i.e. only being able to hold
        //some amount of arrows, etc)
        return false;
    }
    public static bool AddResource(Resource resource)
    {
        switch (resource.resourceType)
        {
            case ResourceType.GOLD:
                return AddGold(resource.amount);
            default:
                return AddGold(resource.amount);
        }
    }

    public static bool AddGold(int amount)
    {
        instance.gold += amount;
        SFXManager.PlaySFX(SFX_TYPE.GOLD_PICKUP);
        HUD.UpdateGoldDisplay(instance.gold);
        return true;
    }

    public static int GetGold()
    {
        return instance.gold;
    }
}
