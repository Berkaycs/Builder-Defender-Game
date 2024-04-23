using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private GameObject _spriteGameObject;
    private ResourcesNearbyOverlay _resourcesNearbyOverlay;

    private void Awake()
    {
        _spriteGameObject = transform.Find("Sprite").gameObject;
        _resourcesNearbyOverlay = transform.Find("ResourceNearbyOverlay").GetComponent<ResourcesNearbyOverlay>();
        Hide();
    }

    private void Start()
    {
        BuildManager.Instance.OnActiveBuildingTypeChanged += BuildManager_OnActiveBuildingTypeChanged;
    }

    private void BuildManager_OnActiveBuildingTypeChanged(object sender, BuildManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        if (e.activeBuildingType == null)
        {
            Hide();
            _resourcesNearbyOverlay.Hide();
        }
        else
        {
            Show(e.activeBuildingType.Sprite);
            _resourcesNearbyOverlay.Show(e.activeBuildingType.ResourceGeneratorData);
        }
    }

    private void Update()
    {
        transform.position = UtilitiesClass.GetMouseWorldPosition();
    }

    private void Show(Sprite ghostSprite)
    {
        _spriteGameObject.SetActive(true);
        _spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }

    private void Hide()
    {
        _spriteGameObject.SetActive(false);
    }
}
