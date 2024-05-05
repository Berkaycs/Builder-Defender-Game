using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance {  get; private set; }

    private CinemachineVirtualCamera _virtualCam;
    private CinemachineBasicMultiChannelPerlin _cinemachineMultiChannelPerlin;

    private float _timer;
    private float _timerMax;
    private float _startingIntensity;

    private void Awake()
    {
        Instance = this;

        _virtualCam = GetComponent<CinemachineVirtualCamera>();
        _cinemachineMultiChannelPerlin = _virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); 
    }

    private void Update()
    {
        if (_timer < _timerMax)
        {
            _timer += Time.deltaTime;
            float amplitude = Mathf.Lerp(_startingIntensity, 0f, _timer / _timerMax);
            _cinemachineMultiChannelPerlin.m_AmplitudeGain = amplitude;
        }
    }

    public void ShakeCamera(float intensity, float timerMax)
    {
        _timerMax = timerMax;
        _timer = 0f;
        _startingIntensity = intensity;
        _cinemachineMultiChannelPerlin.m_AmplitudeGain = intensity; 
    }
}
