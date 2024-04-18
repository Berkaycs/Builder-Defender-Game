using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour
{
    private const string RESOURCE_TYPE_LIST_SO = "ResourceTypeList";

    private ResourceTypeListSO _resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> _resourcesTypeTransform;

    private void Awake()
    {
        _resourceTypeList = Resources.Load<ResourceTypeListSO>(RESOURCE_TYPE_LIST_SO);

        _resourcesTypeTransform = new Dictionary<ResourceTypeSO, Transform>();

        Transform resourceTemplate = transform.Find("ResourceTemplate");
        resourceTemplate.gameObject.SetActive(false);

        int index = 0;
        foreach (ResourceTypeSO resourceType in _resourceTypeList.List)
        {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            resourceTransform.gameObject.SetActive(true);

            float offsetAmount = -160f;
            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            resourceTransform.Find("Image").GetComponent<Image>().sprite = resourceType.Sprite;

            _resourcesTypeTransform[resourceType] = resourceTransform;

            index++;
        }
    }

    private void Start()
    {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
        UpdateResourceAmount();
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs e)
    {
        UpdateResourceAmount();
    }

    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSO resourceType in _resourceTypeList.List)
        {
            Transform resourceTransform = _resourcesTypeTransform[resourceType];

            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
            resourceTransform.Find("Text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
        }
    }
}
