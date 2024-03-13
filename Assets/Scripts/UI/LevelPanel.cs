using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : Singleton<LevelPanel>
{
    public Text timeText;
    public Text enemyText;
    public GameObject enemyTextGroup;
    public TextMeshProUGUI levelNum;

    public void OnEnable()
    {
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
    }
    public void OnDisable()
    {
        EventHandler.EnterLevelEvent -= OnEnterLevelEvent;
    }

    void OnEnterLevelEvent(int l)
    {
        levelNum.text = $"{l}";
    }
    public void TimeChange(int time)
    {
        timeText.text = time+"";
    }

    public void enemyNumChange(int num)
    {
        enemyText.text = num + "";
    }
}
