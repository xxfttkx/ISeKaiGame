using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipsUI : MonoBehaviour
{
    public TextMeshProUGUI tip;
    List<int> tipsTextIndexes = new List<int>() {132,133,134,135,136,137 };
    private void OnEnable()
    {
        Show();
    }
    void Show()
    {
        var i = Random.Range(0, tipsTextIndexes.Count);
        tip.text = SOManager.Instance.GetStringByIndex(tipsTextIndexes[i]);
    }
}
