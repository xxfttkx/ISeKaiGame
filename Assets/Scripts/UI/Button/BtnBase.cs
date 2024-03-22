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
    public bool interactable = true;
    public BtnEvent btnEnter;
    public BtnEvent btnExit;
    public BtnEvent btnClick;
    public BtnBase left;
    public BtnBase right;
    public BtnBase up;
    public BtnBase down;
    

    public virtual void Awake()
    {
        btnEnter.AddListener(BtnEnter);
        btnExit.AddListener(BtnExit);
        btnClick.AddListener(BtnClick);
    }
    protected virtual void OnEnable()
    {
        
    }
    protected virtual void OnDisable()
    {
        if (btnExit != null)
            btnExit.Invoke();
    }
    public virtual void BtnEnter() 
    {

    }
    public virtual void BtnExit() 
    {

    }
    public virtual void BtnClick() 
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable) return;
        if (btnClick != null)
            btnClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!interactable) return;
        if (btnEnter != null)
            btnEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!interactable) return;
        if (btnExit != null)
            btnExit.Invoke();
    }
    public void SetKeyboardRelation(BtnBase l, BtnBase r, BtnBase u, BtnBase d)
    {
        left = l;
        right = r;
        up = u;
        down = d;
    }
    public void SetLeftBtn(BtnBase b)
    {
        left = b;
    }
    public void SetRightBtn(BtnBase b)
    {
        right = b;
    }
    public void SetUpBtn(BtnBase b)
    {
        up = b;
    
    }
    public void SetDownBtn(BtnBase b)
    {
        down = b;
    }
    public void OnEnter()
    {
        btnEnter?.Invoke();
    }
    public void OnExit()
    {
        btnExit?.Invoke();
    }
    public void OnClick()
    {
        btnClick?.Invoke();
    }
}
