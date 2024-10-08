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
            SetSelectedIndex(-1,true);
        }
        else
        {
            SetSelectedIndex(index,true);
        } 
    }
    public void SetExtraSkill(int playerIndex,int extraIndex)
    {
        this.playerIndex = playerIndex;
        var ch = SOManager.Instance.GetPlayerDataByIndex(playerIndex);
        this.extraIndex = extraIndex;
        int curr = SaveLoadManager.Instance.GetPlayerExtraData(playerIndex, ch.extraTypes[extraIndex]);
        int threshold = ch.extraThresholds[extraIndex];
        bool canSelect = curr >= threshold;
#if UNITY_EDITOR
        canSelect = true;
#endif
        string color = canSelect ? "green" : "red";
        string currState = $"(<color={color}>{curr}</color>/{threshold})";
        extraCost.text = Utils.GetExtraString(ch.extraTypes[extraIndex], ch.extraThresholds[extraIndex])+"\n"+currState;
        if(extraIndex==0)
        {
            var c1 = ch.extraCharacteristics[0];
            var c2 = ch.extraCharacteristics[1];
            var v1 = ch.extraCharacteristicVals[0];
            var v2 = ch.extraCharacteristicVals[1];
            extraDesire1.text = $"{ Utils.GetStringByCharacteristicAndVal(c1, v1, true)}\n{ Utils.GetStringByCharacteristicAndVal(c2, v2, false)}";
            extraDesire2.text = $"{ Utils.GetStringByCharacteristicAndVal(c1, v1, false)}\n{ Utils.GetStringByCharacteristicAndVal(c2, v2, true)}";
        }
        else
        {
            extraDesire1.text = SOManager.Instance.GetStringByIndex(ch.extraDesire1[extraIndex]);
            extraDesire2.text = SOManager.Instance.GetStringByIndex(ch.extraDesire2[extraIndex]);
        }
        
        if(!canSelect)
        {
            btns[0].interactable = false;
            btns[1].interactable = false;
        }
        else
        {
            btns[0].interactable = true;
            btns[1].interactable = true;
            btns[0].InitButton(0, TryClickDesire);
            btns[1].InitButton(1, TryClickDesire);
            var selectedIndex = SaveLoadManager.Instance.GetPlayerExtra(playerIndex, extraIndex);
            SetSelectedIndex(selectedIndex - 1);
        }
        
    }
    public void SetSelectedIndex(int index,bool manual = false)
    {
        for (int i = 0; i < btns.Count; ++i)
        {
            if (i == index) btns[i].Select();
            else btns[i].CancelSelect();
        }
        selectedIndex = index;
        if(manual)
        {
            EventHandler.CallDesireChangeEvent(playerIndex, extraIndex, selectedIndex);
        }
        
    }
}
