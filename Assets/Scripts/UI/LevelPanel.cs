using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : Singleton<LevelPanel>
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI enemyText;
    public GameObject enemyTextGroup;
    public TextMeshProUGUI levelNum;

    public void OnEnable()
    {
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
        EventHandler.CreateEnemyTimeChangeEvent += OnCreateEnemyTimeChangeEvent;
        EventHandler.EnemyNumChangeEvent += OnEnemyNumChangeEvent;
    }
    public void OnDisable()
    {
        EventHandler.EnterLevelEvent -= OnEnterLevelEvent;
        EventHandler.EnterLevelEvent -= OnCreateEnemyTimeChangeEvent;
        EventHandler.EnemyNumChangeEvent -= OnEnemyNumChangeEvent;
    }

    void OnEnterLevelEvent(int l)
    {
        levelNum.text = $"{l}";
    }
    public void OnCreateEnemyTimeChangeEvent(int time)
    {
        timeText.text = time+"";
    }

    public void OnEnemyNumChangeEvent(int num)
    {
        enemyText.text = num + "";
    }
}
