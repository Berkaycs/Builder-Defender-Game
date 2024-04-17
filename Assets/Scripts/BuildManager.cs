using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private const string BUILDING_TYPE_LIST_SO = "BuildingTypeList";
    private Camera _mainCamera;
    private BuildingTypeListSO _buildingTypeList;
    private BuildingTypeSO _buildingType;

    // references should be in start method
    private void Start()
    {
        _mainCamera = Camera.main;

        // reference to the building type list
        _buildingTypeList = Resources.Load<BuildingTypeListSO>(BUILDING_TYPE_LIST_SO);
        _buildingType = _buildingTypeList.List[0];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(_buildingType.Prefab, GetMouseWorldPosition(), Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            _buildingType = _buildingTypeList.List[0];
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _buildingType = _buildingTypeList.List[1];
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }
}
