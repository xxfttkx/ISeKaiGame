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
    private int currGlobalIndex; // 当前index
    private int currLevelGlobal; //
    private LevelCreatEnemy levelData;
    public float currLevelTime;

    protected override void Awake()
    {
        base.Awake();
        enemyHash = new HashSet<int>();
        
    }
    private void OnEnable()
    {
        EventHandler.EnterDungeonEvent += OnEnterDungeonEvent;
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        EventHandler.EndLevelEvent += OnEndLevelEvent;
    }
    private void OnDisable()
    {
        EventHandler.EnterDungeonEvent += OnEnterDungeonEvent;
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        EventHandler.EndLevelEvent -= OnEndLevelEvent;
    }
    void OnEnterDungeonEvent(List<int> _)
    {
        currLevelGlobal = 0;
        currGlobalIndex = 0;
    }
    void OnExitLevelEvent(int l)
    {
        currLevelGlobal = currGlobalIndex;
        currEnemyNum = 0;
        currLevelTime = 0;
        timeEnd = false;
        StopAllCoroutines();

    }
    void OnEndLevelEvent(int _)
    {
        
    }
    public void AddEnemyNum(EnemyBase e)
    {
        enemyHash.Add(currGlobalIndex);
        e.SetGlobalIndex(currGlobalIndex);
        e.SetLevelBonus(levelData.bonus);
        currGlobalIndex += 1;
        currEnemyNum += 1;
        EventHandler.CallEnemyNumChangeEvent(currEnemyNum);
    }
    public void SubEnemyNum(int enemyGlobalIndex,bool changeUI)
    {
        if (enemyGlobalIndex != -1)
        {
            enemyHash.Remove(enemyGlobalIndex);
        }
        if (changeUI)
        {
            currEnemyNum -= 1;
            EventHandler.CallEnemyNumChangeEvent(currEnemyNum);

        }
        if (timeEnd && currEnemyNum == 0)
        {
            PassLevel();
        }

    }
    public void StartLevel(int levelIndex)
    {
        EventHandler.CallEnterLevelEvent(levelIndex);
        timeEnd = false;
        EventHandler.CallEnemyNumChangeEvent(currEnemyNum);
        currLevel = levelIndex;
        var l = SOManager.Instance.levelCreatEnemyDataList_SO.GetLevelByIndex(levelIndex);
        levelData = l;
        int n = l.enemyIndex.Length;
        endTime = l.endCreatEnemyTime;
        StartCoroutine(Countdown());
        StartCoroutine(CountLevelTime());
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
            for (int i = n; i < creatEnemyPointList.Count; ++i)
            {
                creatEnemyPointList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < n; ++i)
        {
            creatEnemyPointList[i].Reset(l.enemyIndex[i], l.enemyCreateFirstTime[i], l.enemyCreateDeltaTime[i], l.endCreatEnemyTime);
            creatEnemyPointList[i].gameObject.SetActive(true);
        }
    }

    // 过关
    public void PassLevel()
    {
        EventHandler.CallExitLevelEvent(currLevel);
        if (currLevel == Settings.levelMaxNum)
        {
            EventHandler.CallEndLevelEvent(1);
        }
        else
        {
            EventHandler.CallTransitionEvent(currLevel + 1);
        }
    }

    IEnumerator Countdown()
    {
        EventHandler.CallCreateEnemyTimeChangeEvent(endTime);
        while (endTime > 0)
        {
            yield return new WaitForSeconds(1f);
            endTime = endTime - 1;
            EventHandler.CallCreateEnemyTimeChangeEvent(endTime);
        }
        timeEnd = true;
        SubEnemyNum(-1,false);
    }
    IEnumerator CountLevelTime()
    {
        while (true)
        {
            currLevelTime += Time.deltaTime;
            yield return null;
        }
    }
    public void Retry()
    {
        StartLevel(currLevel);
    }
}
