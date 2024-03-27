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
    public BtnBase leftBtn;
    public BtnBase rightBtn;
    public BtnBase upBtn;
    public BtnBase downBtn;
    

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
        btnClick?.Invoke();
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
        leftBtn = l;
        rightBtn = r;
        upBtn = u;
        downBtn = d;
    }
    public void SetLeftBtn(BtnBase b)
    {
        leftBtn = b;
    }
    public void SetRightBtn(BtnBase b)
    {
        rightBtn = b;
    }
    public void SetUpBtn(BtnBase b)
    {
        upBtn = b;
    
    }
    public void SetDownBtn(BtnBase b)
    {
        downBtn = b;
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
