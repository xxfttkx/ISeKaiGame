using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectPanel : Singleton<SelectPanel>
{
    public GameObject selectPrefab;
    public GameObject professionPrefab;
    private Dictionary<int,SelectPlayerButton> buttons;
    public List<List<BtnBase>> allPlayerButtons;
    public List<int> selectedIndexes;
    private Dictionary<Profession, GameObject> professionToPlayers;

    protected override void Awake()
    {
        base.Awake();
        buttons = new Dictionary<int, SelectPlayerButton>();
    }
    public bool TrySelectPlayer(int index)
    {
        if (CanSelectChar())
        {
            SelectPlayer(index);
            buttons[index].Select();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Init()
    {
        // ¿Õ¸ñÑ¡½ÇÉ«
        allPlayerButtons = new List<List<BtnBase>>();
        for (int i = 0; i < (int)Profession.Max; ++i)
        {
            allPlayerButtons.Add(new List<BtnBase>());
        }

        professionToPlayers = new Dictionary<Profession, GameObject>();
        for (int i = 0; i < (int)Profession.Max; ++i)
        {
            var go = Instantiate(professionPrefab, this.transform);
            professionToPlayers.Add((Profession)i, go);
        }
        foreach (var c in SOManager.Instance.characterDataList_SO.characters)
        {
            if (!c.finished) continue;
            var go  = Instantiate(selectPrefab, professionToPlayers[c.profession].transform);
            var b = go.GetComponent<SelectPlayerButton>();
            buttons.Add(c.index,b);
            b.InitButton(c.index, c.creature.sprite);
            allPlayerButtons[(int)c.profession].Add(b);
        }
        InitKeyboardRelation();
        selectedIndexes = SaveLoadManager.Instance.GetLastCharsIndexes();
        if (selectedIndexes == null || selectedIndexes.Count == 0)
        {
            selectedIndexes = Enumerable.Repeat(-1, Settings.playerMaxNum).ToList();
        }
        else
        {
            InitSeletedChars();
            ButtonPanel.Instance.ChangeLevelInputField(selectedIndexes);
        }
    }
    private void InitSeletedChars()
    {
        for (int i = 0; i < selectedIndexes.Count; ++i)
        {
            var charIndex = selectedIndexes[i];
            
            SlotPanel.Instance.Select(i, charIndex);
            if (charIndex == -1) continue;
            buttons[charIndex].Select();
        }
    }
    public void SelectPlayer(int playerIndex)
    {
        for(int i = 0;i< selectedIndexes.Count;++i)
        {
            if(selectedIndexes[i]==-1)
            {
                selectedIndexes[i] = playerIndex;
                SlotPanel.Instance.Select(i, playerIndex);
                break;
            }
        }
        ButtonPanel.Instance.ChangeLevelInputField(selectedIndexes);
    }
    public void CancelSelectPlayer(int playerIndex)
    {
        for (int i = 0; i < selectedIndexes.Count; ++i)
        {
            if (selectedIndexes[i] == playerIndex)
            {
                selectedIndexes[i] = -1;
                buttons[playerIndex].CancelSelect();
                SlotPanel.Instance.CancelSelect(i);
                break;
            }
        }
        ButtonPanel.Instance.ChangeLevelInputField(selectedIndexes);
    }
    public bool CanSelectChar()
    {
        for (int i = 0; i < selectedIndexes.Count; ++i)
        {
            if (selectedIndexes[i] == -1) return true;
        }
        return false;
    }

    public List<int> GetSelectedIndexes()
    {
        return selectedIndexes;
    }

    public void ClearAllSelect()
    {
        for (int i = 0; i < selectedIndexes.Count; ++i)
        {
            if (selectedIndexes[i] != -1) CancelSelectPlayer(selectedIndexes[i]);
        }
    }
    void InitKeyboardRelation()
    {
        clearBtn.SetDownBtn(allPlayerButtons[0][0]);
        for (int i = 0; i < allPlayerButtons.Count; ++i)
        {
            for (int j = 0; j < allPlayerButtons[i].Count; ++j)
            {
                allPlayerButtons[i][j].SetKeyboardRelation(GetBtnByXY(i - 1, j), GetBtnByXY(i + 1, j), GetBtnByXY(i, j + 1), GetBtnByXY(i, j - 1));
            }
        }
        goBtn.SetLeftBtn(allPlayerButtons[allPlayerButtons.Count-1][allPlayerButtons[allPlayerButtons.Count - 1].Count-1]);
    }
    public BtnBase goBtn;
    public BtnBase clearBtn;
    BtnBase GetBtnByXY(int i,int j)
    {
        int n = allPlayerButtons.Count;
        if (i < 0) return clearBtn;
        if (i > n) return null;
        if (i == n) return goBtn;
        int m = allPlayerButtons[i].Count;
        if (j < 0) return null;
        if (j > m) return null;
        if (j == m) return goBtn;
        return allPlayerButtons[i][j];
    }
}
