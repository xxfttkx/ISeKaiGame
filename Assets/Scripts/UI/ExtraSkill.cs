using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExtraSkill : MonoBehaviour
{
    public TextMeshProUGUI extraCost;
    public TextMeshProUGUI extraDesire1;
    public TextMeshProUGUI extraDesire2;
    public List<SelectDesireButton> btns;
    private int extraIndex;
    private int playerIndex;
    private int selectedIndex;
    private void Awake()
    {
        selectedIndex = -1;
    }
    public void TryClickDesire(int index)
    {
        if(selectedIndex==index)
        {
            SetSelectedIndex(-1);
        }
        else
        {
            SetSelectedIndex(index);
        } 
    }
    public void SetExtraSkill(int playerIndex,int extraIndex)
    {
        this.playerIndex = playerIndex;
        var ch = SOManager.Instance.GetPlayerDataByIndex(playerIndex);
        this.extraIndex = extraIndex;
        extraCost.text = Utils.GetExtraString(ch.extraTypes[extraIndex], ch.extraThresholds[extraIndex]);
        extraDesire1.text = ch.extraDesire1[extraIndex];
        extraDesire2.text = ch.extraDesire2[extraIndex];
        btns[0].InitButton(0, TryClickDesire);
        btns[1].InitButton(1, TryClickDesire);
        var selectedIndex = SaveLoadManager.Instance.GetPlayerExtra(playerIndex, extraIndex);
        SetSelectedIndex(selectedIndex);
    }
    public void SetSelectedIndex(int index)
    {
        for (int i = 0; i < btns.Count; ++i)
        {
            if (i == index) btns[i].Select();
            else btns[i].CancelSelect();
        }
        selectedIndex = index;
        SaveLoadManager.Instance.SavePlayerExtra(playerIndex, extraIndex, selectedIndex);
    }
}
