using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankData : MonoBehaviour
{
    public List<Image> images;
    public TextMeshProUGUI rankNum;
    public TextMeshProUGUI levelNum;
    public TextMeshProUGUI userName;

    public void Init(List<int> indexes,int r,int l,string s = null)
    {
        userName.text = s == null ? "" : s;
        rankNum.text = $"{r}";
        levelNum.text = $"{l}";
        for(int i=0;i<images.Count;++i)
        {
            if(i<indexes.Count)
            {
                images[i].sprite = SOManager.Instance.GetPlayerSpriteSquareByIndex(indexes[i]);
                images[i].enabled = true;
            }
            else
            {
                images[i].enabled = false;
            }
        }
    }
}
