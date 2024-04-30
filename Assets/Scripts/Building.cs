using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO _buildingType;
    private HealthSystem _healthSystem;

    private void Start()
    {
        _buildingType = GetComponent<BuildingTypeHolder>().BuildingType;

        _healthSystem = GetComponent<HealthSystem>();

        _healthSystem.SetHealthAmountMax(_buildingType.HealthAmountMax, true);

        _healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }
}
