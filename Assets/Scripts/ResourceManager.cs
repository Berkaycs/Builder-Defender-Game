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

        TestLogResourceAmontDictionary();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(RESOURCE_TYPE_LIST_SO);
            AddResource(resourceTypeList.List[0], 2);
            TestLogResourceAmontDictionary();
        }
    }

    private void TestLogResourceAmontDictionary()
    {
        foreach (ResourceTypeSO resourceType in _resourceAmountDictionary.Keys)
        {
            Debug.Log(resourceType.Name + ": " + _resourceAmountDictionary[resourceType]);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        _resourceAmountDictionary[resourceType] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

        TestLogResourceAmontDictionary();
    }

    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return _resourceAmountDictionary[resourceType];
    }
}
