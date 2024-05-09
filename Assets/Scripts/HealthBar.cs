using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem _healthSystem;

    private Transform _barTransform;
    private Transform _separatorContainer;

    private void Awake()
    {
        _barTransform = transform.Find("Bar");
    }

    private void Start()
    {
        _separatorContainer = transform.Find("SeparatorContainer");
        ConstructHealthBarSeparators();

        _healthSystem.OnDamaged += HealthSystem_OnDamaged;
        _healthSystem.OnHealed += HealthSystem_OnHealed;
        _healthSystem.OnHealthAmountMaxChanged += HealthSystem_OnHealthAmountMaxChanged;

        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_OnHealthAmountMaxChanged(object sender, System.EventArgs e)
    {
        ConstructHealthBarSeparators();
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void ConstructHealthBarSeparators()
    {
        Transform separatorTemplate = _separatorContainer.Find("SeparatorTemplate");
        separatorTemplate.gameObject.SetActive(false);

        foreach (Transform separatorTransform in _separatorContainer)
        {
            if (separatorTransform == separatorTemplate)
            {
                continue;
            }
            Destroy(separatorTransform.gameObject);
        }

        int healthAmountPerSeparator = 10;
        float barSize = 2f;
        float barOneHealthAmountSize = barSize / _healthSystem.GetHealthAmountMax();
        int healthSeparatorCount = Mathf.FloorToInt(_healthSystem.GetHealthAmountMax() / healthAmountPerSeparator);

        for (int i = 1; i < healthSeparatorCount; i++) // we don't want to place a separator to beginning of health
        {
            Transform separatorTransform = Instantiate(separatorTemplate, _separatorContainer);
            separatorTransform.gameObject.SetActive(true);
            separatorTransform.localPosition = new Vector3(barOneHealthAmountSize * i * healthAmountPerSeparator, 0, 0);
        }
    }

    private void UpdateBar()
    {
        _barTransform.localScale = new Vector3(_healthSystem.GetHealthAmountNormalized(), 1, 1);
    }

    private void UpdateHealthBarVisible()
    {
        if (_healthSystem.IsFullHealth())
        {
            gameObject.SetActive(false);
        }

        else
        {
            gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }
}
