using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Gradient _gradient;

    private Light2D _light2D;
    private float _dayTime;
    private float _dayTimeSpeed;
    private float _secondsPerDay = 30f;


    private void Awake()
    {
        _light2D = GetComponent<Light2D>();
        _dayTimeSpeed = 1 / _secondsPerDay;
    }

    private void Update()
    {
        _dayTime += Time.deltaTime * _dayTimeSpeed;
        _light2D.color = _gradient.Evaluate(_dayTime % 1f);
    }
}
