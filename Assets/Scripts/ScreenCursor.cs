using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCursor : MonoBehaviour {
    public float BlinkPeriod;
    private Image _image;
    private float _elapsedTime = 0;
    private Monitor _monitorScript;
    private Vector2 _lastPosition;
    private RectTransform _rectTransform;

	// Use this for initialization
	void Start () {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _monitorScript = transform.parent.parent.parent.parent.GetComponent<Monitor>();
        _lastPosition = new Vector2();
    }
	
	// Update is called once per frame
	void Update () {
        Blink();
        SetPosition();
    }

    public void ResetTimer()
    {
        _image.enabled = true;
        _elapsedTime = 0;
    }

    void Blink()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > BlinkPeriod)
        {
            _image.enabled = !_image.enabled;
            _elapsedTime = 0;
        }
    }

    void SetPosition()
    {
        Vector2 position = _monitorScript.CurrentPosition;        
        if (!_lastPosition.Equals(position))
        {
            float x = position.x / (_monitorScript.width + 1);
            float y = position.y / 23 * -0.9447f;

            _rectTransform.anchoredPosition = new Vector3(x, y, _rectTransform.localPosition.z);
            _lastPosition = position;
        }        
    }
}
