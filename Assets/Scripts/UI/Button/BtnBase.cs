using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class BtnEvent : UnityEvent
{
}

public class BtnBase : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public BtnEvent btnEnter;
    public BtnEvent btnExit;
    public BtnEvent btnClick;
    
    public virtual void Awake()
    {
        btnEnter.AddListener(BtnEnter);
        btnExit.AddListener(BtnExit);
        btnClick.AddListener(BtnClick);
    }
    public virtual void BtnEnter() { }
    public virtual void BtnExit() { }
    public virtual void BtnClick() { }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (btnClick != null)
            btnClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (btnEnter != null)
            btnEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (btnExit != null)
            btnExit.Invoke();
    }
}
