using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private GameObject _spriteGameObject;

    private void Awake()
    {
        _spriteGameObject = transform.Find("Sprite").gameObject;

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
        }
        else
        {
            Show(e.activeBuildingType.Sprite);
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
