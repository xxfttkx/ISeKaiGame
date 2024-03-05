using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIBtnScaleEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField, Header("����ʱ��С�����٣�"), Range(0, 2)]
    private float _downScale = 0.85f;
    [SerializeField, Header("���ű仯����ʱ�䣺���¹���")]
    private float _downDuration = 0.2f;
    [SerializeField, Header("���ű仯����ʱ�䣺̧�����")]
    private float _upDuration = 0.15f;
    private Button _button;
    private Button button
    {
        get
        {
            if(_button ==null)
            {
                _button = GetComponent<Button>();
            }
            return _button;
        }
    }

    private RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    private RectTransform _rectTransform;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable) return;
        StopAllCoroutines();
        StartCoroutine(ChangeScaleCoroutine(1, _downScale, _downDuration));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScaleCoroutine(RectTransform.localScale.x, 1, _upDuration));
    }

    private IEnumerator ChangeScaleCoroutine(float beginScale, float endScale, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            RectTransform.localScale = Vector3.one * Mathf.Lerp(beginScale, endScale, timer / duration);
            timer += Time.fixedDeltaTime;
            yield return null;
        }
        RectTransform.localScale = Vector3.one * endScale;
    }

    private void OnDisable()
    {
        RectTransform.localScale = Vector3.one;
    }
}