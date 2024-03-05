using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPStatic : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    public int index;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        Init();
    }

    void Init()
    {
        tmp.text = SOManager.Instance.GetStringByIndex(index);
    }

    private void OnChange()
    {

    }
}
