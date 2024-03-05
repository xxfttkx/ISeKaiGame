using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class Contents
{
    public List<Button> contents;
}

public class ButtonCtlBase : MonoBehaviour
{
    private bool isShow;
    [SerializeField]
    private List<Contents> buttons;
    private int currX;
    private int currY;
    private Button cuttButton;
    private bool useKeyboard;
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
        useKeyboard = false;
        currX = 0;
        currY = 0;
        cuttButton = null;
    }
    void Update()
    {
        if (!isShow) return;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ClickCurrButton();
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.W))
        {
            PretendClickButton(1);
            return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            PretendClickButton(2);
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            PretendClickButton(3);
            return;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            PretendClickButton(4);
            return;
        }
    }

    // 1-up 2-down 3-left 4-right    buttons[i].Count>0--default
    void PretendClickButton(int command)
    {
        do
        {
            if (!useKeyboard)
            {
                useKeyboard = true;
                break;
            }
            if (command == 1)
            {
                if (currX == 0) return;
                currX = currX - 1;
                if (!IsButtonExist(currX, currY)) currY = buttons[currX].contents.Count - 1;
                break;
            }
            if (command == 2)
            {
                if (currX == buttons.Count-1) return;
                currX = currX + 1;
                if (!IsButtonExist(currX, currY)) currY = buttons[currX].contents.Count - 1;
                break;
            }
            if (command == 3)
            {
                if (currY == 0) return;
                currY = currY - 1;
                break;
            }
            if (command == 4)
            {
                if (currY == buttons[currX].contents.Count-1) return;
                currY = currY + 1;
                break;
            }
        } while (false);
        
        Button b = buttons[currX].contents[currY];
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        cuttButton?.OnPointerUp(pointerEventData);
        b.OnPointerDown(pointerEventData);
        cuttButton = b;

    }
    void ClickCurrButton()
    {
        if (!useKeyboard) return;
        if(IsButtonExist(currX,currY))
        {
            Button b = buttons[currX].contents[currY];
            b.onClick.Invoke();
        }
    }
    bool IsButtonExist(int x, int y)
    {
        if (x < 0 || y < 0) return false;
        if (buttons != null && buttons.Count > currX && buttons[currX].contents.Count > currY)
        {
            return true;
        }
        return false;
    }
}
