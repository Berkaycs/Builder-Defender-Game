using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private float _orthographicSize;
    private float _targetOrthographicSize;
    private bool _edgeScrolling;

    private void Awake()
    {
        Instance = this;

        _edgeScrolling = PlayerPrefs.GetInt("edgeScrolling", 1) == 1;
    }
        
    private void Start()
    {
        _orthographicSize = _cinemachineVirtualCamera.m_Lens.OrthographicSize;
        _targetOrthographicSize = _orthographicSize;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (_edgeScrolling)
        {
            float edgeScrollingSize = 30;
            if (Input.mousePosition.x > Screen.width - edgeScrollingSize)
            {
                x += 1f;
            }

            if (Input.mousePosition.x < edgeScrollingSize)
            {
                x -= 1f;
            }

            if (Input.mousePosition.y > Screen.height - edgeScrollingSize)
            {
                y += 1f;
            }

            if (Input.mousePosition.y < edgeScrollingSize)
            {
                y -= 1f;
            }
        }
        
        Vector3 moveDir = new Vector3(x, y, 0).normalized;
        float moveSpeed = 30f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float zoomAmount = 2f;
        _targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;

        float minOrthographicSize = 10f;
        float maxOrthographicSize = 30f;
        _targetOrthographicSize = Mathf.Clamp(_targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

        float zoomSpeed = 5f;
        _orthographicSize = Mathf.Lerp(_orthographicSize, _targetOrthographicSize, Time.deltaTime * zoomSpeed);

        _cinemachineVirtualCamera.m_Lens.OrthographicSize = _orthographicSize;
    }

    public void SetEdgeScrolling(bool edgeScrolling)
    {
        _edgeScrolling = edgeScrolling;
        PlayerPrefs.SetInt("edgeScrolling", _edgeScrolling ? 1 : 0); // if it's true return 1 else return 0
    }

    public bool GetEdgeScrolling()
    {
        return _edgeScrolling;
    }
}
