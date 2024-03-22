using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCharPanel : Singleton<SelectedCharPanel>
{
    public Image selectedChar;
    private Text desc;

    protected override void Awake()
    {
        base.Awake();
        desc = GetComponentInChildren<Text>();
        desc.text = "";
    }
    public void SelectChar(int playerIndex)
    {
        var ch = SOManager.Instance.characterDataList_SO.characters[playerIndex];
        selectedChar.sprite = ch.creature.sprite;
        desc.text = ch.index+": "+ch.desc;
    }
    public void CancelSelectChar()
    {

    }
}
