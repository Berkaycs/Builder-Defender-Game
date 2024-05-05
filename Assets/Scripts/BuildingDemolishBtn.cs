using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishBtn : MonoBehaviour
{
    [SerializeField] private Building _building;

    private void Awake()
    {
        transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingTypeSO buildingType = _building.GetComponent<BuildingTypeHolder>().BuildingType;

            foreach (ResourceAmount resourceAmount in buildingType.ConstructionResourceCostArray)
            {
                ResourceManager.Instance.AddResource(resourceAmount.ResourceType, Mathf.FloorToInt(resourceAmount.Amount * .6f));
            }

            Destroy(_building.gameObject);
        });
    }
}
