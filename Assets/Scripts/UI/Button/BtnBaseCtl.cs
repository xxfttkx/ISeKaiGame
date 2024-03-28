using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BtnBaseCtl : MonoBehaviour
{
    public bool isShow;
    public BtnBase firstBtn;
    public BtnBase currBtn;
    public bool useKeyboard;

    private void Awake()
    {
        isShow = false;
    }
    private void OnEnable()
    {
        Reset();
        isShow = true;
    }
    private void OnDisable()
    {
        isShow = false;
    }
    public void Reset()
    {
        currBtn = null;
        useKeyboard = false;
    }
    void Update()
    {
        if (!isShow) return;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ClickCurrButton();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            EnterBtn(1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            EnterBtn(2);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            EnterBtn(3);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            EnterBtn(4);
        }
    }

    // 1-up 2-down 3-left 4-right
    void EnterBtn(int command)
    {
        BtnBase lastBtn = currBtn;
        do
        {
            if (currBtn==null)
            {
                currBtn = firstBtn;
                if (currBtn.interactable)
                    break;
            }
            if (command == 1)
            {
                while (currBtn != null)
                {
                    currBtn = currBtn.upBtn;
                    if (currBtn != null && currBtn.interactable)
                        break;
                }
            }
            else if (command == 2)
            {
                while (currBtn != null)
                {
                    currBtn = currBtn.downBtn;
                    if (currBtn != null && currBtn.interactable)
                        break;
                }
            }
            else if (command == 3)
            {
                while (currBtn != null)
                {
                    currBtn = currBtn.leftBtn;
                    if (currBtn != null && currBtn.interactable)
                        break;
                }
            }
            else if (command == 4)
            {
                while (currBtn != null)
                {
                    currBtn = currBtn.rightBtn;
                    if (currBtn != null && currBtn.interactable)
                        break;
                }
            }
        } while (false);
        if (currBtn == null|| !currBtn.interactable)
        {
            currBtn = lastBtn;
        }
        else
        {
            lastBtn?.OnExit();
            currBtn?.OnEnter();
        }


    }
    void ClickCurrButton()
    {
        if (currBtn != null && currBtn.interactable)
        {
            currBtn.OnClick();
        }
    }
    public void SetFirstBtn(BtnBase b)
    {
        firstBtn = b;
    }
}
