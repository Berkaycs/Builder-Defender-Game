using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO _buildingType;
    private HealthSystem _healthSystem;
    private Transform _buildingDemolishBtn;
    private Transform _buildingRepairBtn;

    private void Awake()
    {
        _buildingDemolishBtn = transform.Find("BuildingDemolishBtn");
        _buildingRepairBtn = transform.Find("BuildingRepairBtn");

        HideBuildingDemolishBtn();
    }

    private void Start()
    {
        _buildingType = GetComponent<BuildingTypeHolder>().BuildingType;

        _healthSystem = GetComponent<HealthSystem>();

        _healthSystem.SetHealthAmountMax(_buildingType.HealthAmountMax, true);

        _healthSystem.OnDamaged += HealthSystem_OnDamaged;
        _healthSystem.OnHealed += HealthSystem_OnHealed;
        _healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        if (_healthSystem.IsFullHealth())
        {
            HideBuildingRepairBtn();
        }
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        ShowBuildingRepairBtn();
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
        CinemachineShake.Instance.ShakeCamera(7f, .15f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Instantiate(Resources.Load<Transform>("BuildingDestroyedParticles"), transform.position, Quaternion.identity);
        Destroy(gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
        CinemachineShake.Instance.ShakeCamera(10f, .2f);
    }

    private void OnMouseEnter()
    {
        ShowBuildingDemolishBtn();
    }

    private void OnMouseExit()
    {
        HideBuildingDemolishBtn();
    }

    private void ShowBuildingDemolishBtn()
    {
        if (_buildingDemolishBtn != null)
        {
            _buildingDemolishBtn.gameObject.SetActive(true);
        }
    }

    private void HideBuildingDemolishBtn()
    {
        if (_buildingDemolishBtn != null)
        {
            _buildingDemolishBtn.gameObject.SetActive(false);
        }
    }

    private void ShowBuildingRepairBtn()
    {
        if (_buildingRepairBtn != null)
        {
            _buildingRepairBtn.gameObject.SetActive(true);
        }
    }

    private void HideBuildingRepairBtn()
    {
        if (_buildingRepairBtn != null)
        {
            _buildingRepairBtn.gameObject.SetActive(false);
        }
    }
}
