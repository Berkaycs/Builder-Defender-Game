using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }

    [SerializeField] private RectTransform _canvasRectTransform;

    private RectTransform _rectTransform;
    private RectTransform _backgroundRectTransform;
    private TextMeshProUGUI _textMeshPro;
    private TooltipTimer _tooltipTimer;

    private void Awake()
    {
        Instance = this;

        _rectTransform = GetComponent<RectTransform>();
        _backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        _textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();

        Hide();
    }

    private void Update()
    {
        HandleFollowMouse();

        if (_tooltipTimer != null)
        {
            _tooltipTimer.Timer -= Time.deltaTime;

            if (_tooltipTimer.Timer <= 0)
            {
                Hide();
            }
        }
    }

    private void HandleFollowMouse()
    {
        Vector2 anchoredPosition = Input.mousePosition / _canvasRectTransform.localScale.x;

        // prevent the tooltip to being missed in the corner of the screen
        if (anchoredPosition.x + _backgroundRectTransform.rect.width > _canvasRectTransform.rect.width)
        {
            anchoredPosition.x = _canvasRectTransform.rect.width - _backgroundRectTransform.rect.width;
        }

        if (anchoredPosition.y + _backgroundRectTransform.rect.height > _canvasRectTransform.rect.height)
        {
            anchoredPosition.y = _canvasRectTransform.rect.height - _backgroundRectTransform.rect.height;
        }

        _rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string tooltipText)
    {
        _textMeshPro.SetText(tooltipText);
        _textMeshPro.ForceMeshUpdate();

        Vector2 textSize = _textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8, 8);
        _backgroundRectTransform.sizeDelta = textSize + padding;
    }

    public void Show(string tooltipText, TooltipTimer tooltipTimer = null)
    {
        this._tooltipTimer = tooltipTimer;
        gameObject.SetActive(true);
        SetText(tooltipText);
        HandleFollowMouse();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public class TooltipTimer
    {
        public float Timer;
    }
}
