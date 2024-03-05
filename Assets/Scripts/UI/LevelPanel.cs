using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : Singleton<LevelPanel>
{
    public Text timeText;
    public Text enemyText;
    public GameObject enemyTextGroup;

    public void TimeChange(int time)
    {
        timeText.text = time+"";
    }

    public void enemyNumChange(int num)
    {
        enemyText.text = num + "";
    }
}
