using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    private const string BUILDING_TYPE_LIST_SO = "BuildingTypeList";

    [SerializeField] private Sprite _arrowSprite;
    [SerializeField] private List<BuildingTypeSO> _ignoreBuildingTypeList;

    private Dictionary<BuildingTypeSO, Transform> _btnTransformDictionary;
    private Transform _arrowBtn;

    private void Awake()
    {
        Transform btnTemplate = transform.Find("BtnTemplate");
        btnTemplate.gameObject.SetActive(false);

        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(BUILDING_TYPE_LIST_SO);

        _btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        int index = 0;

        _arrowBtn = Instantiate(btnTemplate, transform);
        _arrowBtn.gameObject.SetActive(true);

        float offsetAmount = +250f;
        _arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

        _arrowBtn.Find("Image").GetComponent<Image>().sprite = _arrowSprite;
        _arrowBtn.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30);

        // creating a func fastly with no parameter by using lambda
        _arrowBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildManager.Instance.SetActiveBuildingType(null);
        });

        MouseEnterExitEvents mouseEnterExitEvents = _arrowBtn.GetComponent<MouseEnterExitEvents>();
        mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Show("Arrow");
        };

        mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Hide();
        };

        index++;

        foreach (BuildingTypeSO buildingType in buildingTypeList.List)
        {
            if (_ignoreBuildingTypeList.Contains(buildingType))
            {
                continue;
            }

            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);

            offsetAmount = +250f;
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            btnTransform.Find("Image").GetComponent<Image>().sprite = buildingType.Sprite;

            // creating a func fastly with no parameter by using lambda
            btnTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildManager.Instance.SetActiveBuildingType(buildingType);
            });

            mouseEnterExitEvents = btnTransform.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Show(buildingType.Name + "\n" + buildingType.GetConstructionResourceCostString());
            };

            mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Hide();
            };

            _btnTransformDictionary[buildingType] = btnTransform;

            index++;
        }
    }

    private void Start()
    {
        BuildManager.Instance.OnActiveBuildingTypeChanged += BuildManager_OnActiveBuildingTypeChanged;
        UpdateBuildingTypeButton();
    }

    private void BuildManager_OnActiveBuildingTypeChanged(object sender, BuildManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        UpdateBuildingTypeButton(); 
    }

    private void UpdateBuildingTypeButton()
    {
        _arrowBtn.Find("Selected").gameObject.SetActive(false);

        foreach (BuildingTypeSO buildingType in _btnTransformDictionary.Keys)
        {
            Transform btnTransform = _btnTransformDictionary[buildingType];
            btnTransform.Find("Selected").gameObject.SetActive(false);
        }

        BuildingTypeSO activeBuildingType = BuildManager.Instance.GetActiveBuildingType();
        if (activeBuildingType == null)
        {
            _arrowBtn.Find("Selected").gameObject.SetActive(true);
        }
        else
        {
            _btnTransformDictionary[activeBuildingType].Find("Selected").gameObject.SetActive(true);
        }
    }
}
