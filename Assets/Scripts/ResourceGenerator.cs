using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private BuildingTypeSO _buildingType;
    private float _timer;
    private float _timerMax;

    private void Awake()
    {
        _buildingType = GetComponent<BuildingTypeHolder>().BuildingType;
        _timerMax = _buildingType.ResourceGeneratorData.TimerMax;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if(_timer <= 0f)
        {
            _timer += _timerMax;
            Debug.Log("Ding! " + _buildingType.ResourceGeneratorData.ResourceType.Name);
            ResourceManager.Instance.AddResource(_buildingType.ResourceGeneratorData.ResourceType, 1);
        }
    }
}
