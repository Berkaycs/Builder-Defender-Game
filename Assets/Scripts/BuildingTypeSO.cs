using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string Name;
    public Transform Prefab;
    public ResourceGeneratorData ResourceGeneratorData;
    public Sprite Sprite;
    public float MinConstructionRadius;
    public ResourceAmount[] ConstructionResourceCostArray;

    public string GetConstructionResourceCostString()
    {
        string str = "";
        foreach (ResourceAmount resourceAmount in ConstructionResourceCostArray)
        {
            str += "<color=#" + resourceAmount.ResourceType.ColorHex + ">" + 
                resourceAmount.ResourceType.ShortName + resourceAmount.Amount + 
                " </color>";
        }
        return str;
    }
}
