using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesNearbyOverlay : MonoBehaviour
{
    private ResourceGeneratorData _resourceGeneratorData;

    private void Awake()
    {
        Hide();
    }

    private void Update()
    {
        int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(_resourceGeneratorData, transform.position - transform.localPosition);
        float percent = Mathf.RoundToInt((float)nearbyResourceAmount / _resourceGeneratorData.MaxResourceAmount * 100f);
        transform.Find("Text").GetComponent<TextMeshPro>().SetText(percent + "%");
    }

    public void Show(ResourceGeneratorData resourceGeneratorData)
    {
        this._resourceGeneratorData = resourceGeneratorData;
        gameObject.SetActive(true);

        transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = _resourceGeneratorData.ResourceType.Sprite;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
