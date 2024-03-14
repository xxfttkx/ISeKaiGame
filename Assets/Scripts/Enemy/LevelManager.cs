using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public GameObject creatEnemyPointPrefab;
    public List<CreateEnemyPoint> creatEnemyPointList;
    public int currLevel;
    public bool timeEnd;
    public int currEnemyNum;
    private int endTime;
    private HashSet<int> enemyHash;
    private int currCount; // 当前index
    private int currGlobal; //

    protected override void Awake()
    {
        base.Awake();
        enemyHash = new HashSet<int>();
        currCount = 0;
        currEnemyNum = 0;
        currGlobal = 0;
    }
    private void OnEnable()
    {
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        EventHandler.EndLevelEvent += OnEndLevelEvent;
    }
    private void OnDisable()
    {
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        EventHandler.EndLevelEvent -= OnEndLevelEvent;
    }
    void OnExitLevelEvent(int l)
    {
        currGlobal = currCount;
        currEnemyNum = 0;
    }
    void OnEndLevelEvent()
    {
        /*currCount = 0;
        currEnemyNum = 0;
        currGlobal = 0;*/
    }
    public void AddEnemyNum(EnemyBase e)
    {
        enemyHash.Add(currCount);
        e.SetGlobalIndex(currCount);
        currCount += 1;
        currEnemyNum += 1;
        LevelPanel.Instance.enemyNumChange(currEnemyNum);
    }
    public void SubEnemyNum(int enemyGlobalIndex)
    {
        if (enemyGlobalIndex != -1)
        {
            enemyHash.Remove(enemyGlobalIndex);
        }
        if (enemyGlobalIndex >= currGlobal)
        {
            currEnemyNum -= 1;
            LevelPanel.Instance.enemyNumChange(currEnemyNum);
            if (timeEnd && currEnemyNum == 0)
            {
                PassLevel();
            }
        }
        
    }
    public void StartLevel(int levelIndex)
    {
        EventHandler.CallEnterLevelEvent(levelIndex);
        timeEnd = false;
        LevelPanel.Instance.enemyNumChange(currEnemyNum);
        currLevel = levelIndex;
        var l = SOManager.Instance.levelCreatEnemyDataList_SO.GetLevelByIndex(levelIndex);
        int n = l.offset.Length;
        endTime = l.endCreatEnemyTime;
        LevelPanel.Instance.TimeChange(endTime);
        StopAllCoroutines();
        StartCoroutine(Countdown());
        if (creatEnemyPointList.Count < n)
        {
            for (int i = creatEnemyPointList.Count; i < n; ++i)
            {
                var go = Instantiate(creatEnemyPointPrefab, this.transform);
                creatEnemyPointList.Add(go.GetComponent<CreateEnemyPoint>());
            }
        }
        else if (creatEnemyPointList.Count > n)
        {
            for (int i = creatEnemyPointList.Count; i < n; ++i)
            {
                creatEnemyPointList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < n; ++i)
        {
            creatEnemyPointList[i].Reset(l.offset[i], l.enemyIndex[i], l.enemyCreateFirstTime[i], l.enemyCreateDeltaTime[i], l.endCreatEnemyTime);
            creatEnemyPointList[i].gameObject.SetActive(true);
        }
    }

    // 过关
    public void PassLevel()
    {
        List<int> charIndexes = PlayerManager.Instance.GetCharsIndexes();
        SaveLoadManager.Instance.PassLevel(charIndexes, currLevel);
        EventHandler.CallExitLevelEvent(currLevel);
        if (currLevel == Settings.levelMaxNum)
        {
            EndCanvas.Instance.EndGame(1);
        }
        else
        {
            EventHandler.CallTransitionEvent(currLevel+1);
        }


    }

    IEnumerator Countdown()
    {
        while (endTime > 0)
        {
            yield return new WaitForSeconds(1f);
            endTime = endTime - 1;
            LevelPanel.Instance.TimeChange(endTime);
        }
        timeEnd = true;
        SubEnemyNum(-1);
    }
    public void Retry()
    {
        StartLevel(currLevel);
    }
}
