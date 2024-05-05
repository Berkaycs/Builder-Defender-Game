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

    [SerializeField] private Building _hqBuilding;

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

        _hqBuilding.GetComponent<HealthSystem>().OnDied += BuildManager_OnDied;
    }

    private void Update()
    {
        // if there is an UI in the screen where you click, it does not instantiate the buildings 
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (_activeBuildingType != null)
            {
                if (CanSpawnBuilding(_activeBuildingType, UtilitiesClass.GetMouseWorldPosition(), out string errorMessage))
                {
                    if (ResourceManager.Instance.CanAfford(_activeBuildingType.ConstructionResourceCostArray))
                    {
                        ResourceManager.Instance.SpendResources(_activeBuildingType.ConstructionResourceCostArray);
                        //Instantiate(_activeBuildingType.Prefab, UtilitiesClass.GetMouseWorldPosition(), Quaternion.identity);
                        BuildingConstruction.Create(UtilitiesClass.GetMouseWorldPosition(), _activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                    }
                    else
                    {
                        TooltipUI.Instance.Show("Cannot afford " + _activeBuildingType.GetConstructionResourceCostString(),
                            new TooltipUI.TooltipTimer { Timer = 2f });
                    }
                }
                else
                {
                    TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer { Timer = 2f });
                }
            }         
        }
    }

    private void BuildManager_OnDied(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
        GameOverUI.Instance.Show();
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

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    {
        BoxCollider2D boxCollider2D = buildingType.Prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear)
        {
            errorMessage = "Area is not clear!";
            return false;
        }

        if (buildingType.HasResourceGeneratorData)
        {
            ResourceGeneratorData resourceGeneratorData = buildingType.ResourceGeneratorData;
            int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, position);

            if (nearbyResourceAmount == 0)
            {
                errorMessage = "There are no nearby Resource Nodes!";
                return false;
            }
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
                    errorMessage = "Too close to another building of the same type!";
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
                errorMessage = "";
                return true;
            }
        }
        errorMessage = "Too far from any other building!";
        return false;
    }

    public Building GetHQBuilding()
    {
        return _hqBuilding;
    }
}
