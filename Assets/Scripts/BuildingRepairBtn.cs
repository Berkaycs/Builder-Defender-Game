using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairBtn : MonoBehaviour
{
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private ResourceTypeSO _goldResourceType;

    private void Awake()
    {
        transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            int missingHealth = _healthSystem.GetHealthAmountMax() - _healthSystem.GetHealthAmount();
            int repairCost = missingHealth / 2;

            ResourceAmount[] resourceAmountCost = new ResourceAmount[] {
                new ResourceAmount { ResourceType = _goldResourceType, Amount = repairCost} };

            if (ResourceManager.Instance.CanAfford(resourceAmountCost))
            {
                ResourceManager.Instance.SpendResources(resourceAmountCost);
                _healthSystem.HealFull();
            }
            else
            {
                TooltipUI.Instance.Show("You have no enough gold!", new TooltipUI.TooltipTimer { Timer = 2f});
            }
        });
    }
}
