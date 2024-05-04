using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private ResourceGeneratorData _resourceGeneratorData;
    private float _timer;
    private float _timerMax;

    public static int GetNearbyResourceAmount(ResourceGeneratorData _resourceGeneratorData, Vector3 position)
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, _resourceGeneratorData.ResourceDetectionRadius);

        int nearbyResourceAmount = 0;
        foreach (Collider2D collider2D in collider2DArray)
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null)
            {
                // It's a resource node!
                if (resourceNode.ResourceType == _resourceGeneratorData.ResourceType)
                {
                    // Same type!
                    nearbyResourceAmount++;
                }
            }
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, _resourceGeneratorData.MaxResourceAmount);

        return nearbyResourceAmount;
    }

    private void Awake()
    {
        _resourceGeneratorData = GetComponent<BuildingTypeHolder>().BuildingType.ResourceGeneratorData;
        _timerMax = _resourceGeneratorData.TimerMax;
    }

    private void Start()
    {
        int nearbyResourceAmount = GetNearbyResourceAmount(_resourceGeneratorData, transform.position);

        if (nearbyResourceAmount == 0)
        {
            // No resource nodes nearby
            // Disable resource generator
            enabled = false;
        }

        else
        {
            _timerMax = (_resourceGeneratorData.TimerMax / 2f) + 
                _resourceGeneratorData.TimerMax * 
                (1 - (float)nearbyResourceAmount / _resourceGeneratorData.MaxResourceAmount);
        }
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if(_timer <= 0f)
        {
            _timer += _timerMax;
            ResourceManager.Instance.AddResource(_resourceGeneratorData.ResourceType, 1);
        }
    }

    public ResourceGeneratorData GetResourceGeneratorData()
    {
        return _resourceGeneratorData;
    }

    public float GetTimerNormalized()
    {
        return _timer / _timerMax;
    }

    public float GetAmountGeneratedPerSecond()
    {
        return 1 / _timerMax;
    }
}
