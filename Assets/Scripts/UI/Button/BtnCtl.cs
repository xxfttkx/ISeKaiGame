using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class BtnRow
{
    public List<BtnBase> buttons;
}

public class BtnCtl : MonoBehaviour
{
    private bool isShow;
    [SerializeField]
    private List<BtnRow> btnRows;
    private int currX;
    private int currY;
    private BtnBase cuttButton;
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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) 
        {
            ClickCurrButton();
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            EnterBtn(1);
            return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            EnterBtn(2);
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            EnterBtn(3);
            return;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            EnterBtn(4);
            return;
        }
    }

    // 1-up 2-down 3-left 4-right    buttons[i].Count>0--default
    void EnterBtn(int command)
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
                if (!IsButtonExist(currX, currY)) currY = btnRows[currX].buttons.Count - 1;
                break;
            }
            if (command == 2)
            {
                if (currX == btnRows.Count - 1) return;
                currX = currX + 1;
                if (!IsButtonExist(currX, currY)) currY = btnRows[currX].buttons.Count - 1;
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
                if (currY == btnRows[currX].buttons.Count - 1) return;
                currY = currY + 1;
                break;
            }
        } while (false);

        BtnBase b = btnRows[currX].buttons[currY];
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        cuttButton?.OnPointerExit(pointerEventData);
        b.OnPointerEnter(pointerEventData);
        cuttButton = b;

    }
    void ClickCurrButton()
    {
        if (!useKeyboard) return;
        if (IsButtonExist(currX, currY))
        {
            BtnBase b = btnRows[currX].buttons[currY];
            b.btnClick.Invoke();
        }
    }
    bool IsButtonExist(int x, int y)
    {
        if (x < 0 || y < 0) return false;
        if (btnRows != null && btnRows.Count > currX && btnRows[currX].buttons.Count > currY)
        {
            return true;
        }
        return false;
    }

    public void SetBtnRows(List<List<BtnBase>> list)
    {
        btnRows = new List<BtnRow>(list.Count);
        for (int i = 0; i < list.Count; ++i)
        {
            var btnRow = new BtnRow();
            btnRows.Add(btnRow);
            btnRow.buttons = list[i];
        }
    }
}
