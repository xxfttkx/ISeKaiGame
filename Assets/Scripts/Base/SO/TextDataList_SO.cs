using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextDataList_SO", menuName = "Custom/TextDataList_SO")]
public class TextDataList_SO : ScriptableObject
{
    public List<LanguageToText> textDataList;

    public string GetTextString(int key,int languageIndex)
    {
        string ans = "";
        if (key >= textDataList.Count) return ans;
        if (languageIndex >= textDataList[key].texts.Count)
        {
            languageIndex = 1;
        }
        ans = textDataList[key].texts[languageIndex];
        if(ans=="")ans= textDataList[key].texts[1];//default english
        return ans;
    }
}

