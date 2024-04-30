using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private const string RESOURCE_TYPE_LIST_SO = "ResourceTypeList";
    private Dictionary<ResourceTypeSO, int> _resourceAmountDictionary;

    public event EventHandler OnResourceAmountChanged;

    // initialization should be in awake method
    private void Awake()
    {
        Instance = this;

        _resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(RESOURCE_TYPE_LIST_SO);

        foreach (ResourceTypeSO resourceType in resourceTypeList.List)
        {
            _resourceAmountDictionary[resourceType] = 0;
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        _resourceAmountDictionary[resourceType] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return _resourceAmountDictionary[resourceType];
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            if (GetResourceAmount(resourceAmount.ResourceType) >= resourceAmount.Amount)
            {
                // can afford
            }
            else
            {
                // can not afford this!
                return false;
            }
        }
        return true;
    }

    public void SpendResources(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            _resourceAmountDictionary[resourceAmount.ResourceType] -= resourceAmount.Amount;
        }
    }
}
