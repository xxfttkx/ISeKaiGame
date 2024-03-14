using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class EscEvent : UnityEvent
{
}

public class ShadowImage : MonoBehaviour, IPointerClickHandler
{
    private Image shadow;
    public EscEvent escEvent;
    private void Awake()
    {
        shadow = GetComponent<Image>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (escEvent != null)
            escEvent.Invoke();
        // UIManager.Instance.EscOnePanel();
    }
}
