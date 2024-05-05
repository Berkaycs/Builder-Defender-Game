using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionTimerUI : MonoBehaviour
{
    [SerializeField] private BuildingConstruction _buildingConstruction;

    private Image _constructionProgressImage;

    private void Awake()
    {
        _constructionProgressImage = transform.Find("Mask").Find("Image").GetComponent<Image>();
    }

    private void Update()
    {
        _constructionProgressImage.fillAmount = _buildingConstruction.GetConstructionTimerNormalized();
    }
}
