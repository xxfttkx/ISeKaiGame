using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCharPanel : Singleton<SelectedCharPanel>
{
    public Image selectedChar;
    private TextMeshProUGUI desc;

    protected override void Awake()
    {
        base.Awake();
        desc = GetComponentInChildren<TextMeshProUGUI>();
        desc.text = "";
    }
    public void SelectChar(int playerIndex)
    {
        var ch = SOManager.Instance.characterDataList_SO.characters[playerIndex];
        selectedChar.sprite = ch.creature.sprite;
        desc.text = $"{ch.index}:\n{SOManager.Instance.GetStringByIndex(ch.desc)}";
    }
    public void CancelSelectChar()
    {

    }
}
