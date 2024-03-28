using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DataRaw : MonoBehaviour
{
    public int playerIndex;
    public List<TextMeshProUGUI> tmpList;
    public Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void Init(int i)
    {
        playerIndex = i;
        image.sprite = SOManager.Instance.GetPlayerSpriteSquareByIndex(i);
        var list = SaveLoadManager.Instance.GetPlayerExtraDataList(i);
        for (int j = 0; j < list.Count; ++j)
        {
            tmpList[j].text = list[j] + "";
        }
    }
}
