using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType)
    {
        Transform constructionPrefab = Resources.Load<Transform>("BuildingConstruction");
        Transform constructionTransform = Instantiate(constructionPrefab, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = constructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(3f, buildingType);

        return buildingConstruction;
    }

    private BuildingTypeSO _buildingType;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _spriteRenderer;
    private BuildingTypeHolder _buildingTypeHolder;
    private Material _constructionMaterial;

    private float _constructionTimer;
    private float _constructionTimerMax;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        _constructionMaterial = _spriteRenderer.material;
    }

    private void Update()
    {
        _constructionTimer -= Time.deltaTime;

        _constructionMaterial.SetFloat("_Progress", GetConstructionTimerNormalized());

        if (_constructionTimer < 0f)
        {
            Debug.Log("Ding!");
            Instantiate(_buildingType.Prefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
            Destroy(gameObject);
        }
    }

    private void SetBuildingType(float constructionTimerMax, BuildingTypeSO buildingType)
    {
        this._buildingType = buildingType;

        _constructionTimerMax = buildingType.ConstructionTimerMax;
        _constructionTimer = constructionTimerMax;

        _spriteRenderer.sprite = buildingType.Sprite;

        _boxCollider.offset = buildingType.Prefab.GetComponent<BoxCollider2D>().offset;
        _boxCollider.size = buildingType.Prefab.GetComponent <BoxCollider2D>().size;

        _buildingTypeHolder.BuildingType = buildingType;
    }

    public float GetConstructionTimerNormalized()
    {
        return 1 - _constructionTimer / _constructionTimerMax;
    }
}
