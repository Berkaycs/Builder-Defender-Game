using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }

    private const string BUILDING_TYPE_LIST_SO = "BuildingTypeList";
    private Camera _mainCamera;
    private BuildingTypeListSO _buildingTypeList;
    private BuildingTypeSO _activeBuildingType;

    // references should be in start method
    private void Awake()
    {
        Instance = this;

        // reference to the building type list
        _buildingTypeList = Resources.Load<BuildingTypeListSO>(BUILDING_TYPE_LIST_SO);
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        // if there is an UI in the screen where you click, it does not instantiate the buildings 
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (_activeBuildingType != null && CanSpawnBuilding(_activeBuildingType, UtilitiesClass.GetMouseWorldPosition()))
            {
                Instantiate(_activeBuildingType.Prefab, UtilitiesClass.GetMouseWorldPosition(), Quaternion.identity);
            }
        }
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        _activeBuildingType = buildingType;

        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = _activeBuildingType });
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return _activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position)
    {
        BoxCollider2D boxCollider2D = buildingType.Prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear)
        {
            return false;
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.MinConstructionRadius);

        // just can build one of each building type in the same area
        foreach (Collider2D collider2D in collider2DArray)
        {
            // Colliders inside the construction radius
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                // Has a BuildingTypeHolder
                if (buildingTypeHolder.BuildingType == buildingType)
                {
                    // There's already a building of this type within the construction radius!
                    return false;
                }
            }
        }

        float maxConstructionRadius = 25f;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            // Colliders inside the construction radius
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                // It's a building!
                return true;
            }
        }

        return false;
    }
}
