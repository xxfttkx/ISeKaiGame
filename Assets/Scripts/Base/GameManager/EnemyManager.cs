using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public Queue<EnemyBase> toAtkEnemies;

    protected override void Awake()
    {
        base.Awake();
        toAtkEnemies = new Queue<EnemyBase>();
    }
    private void OnEnable()
    {
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
    }
    private void OnDisable()
    {
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
    }
    void OnEnterLevelEvent(int _)
    {
        StartCoroutine(EnemiesAtk());
    }
    void OnExitLevelEvent(int _)
    {
        StopAllCoroutines();
        toAtkEnemies.Clear();
    }
    public void AddEnemyToAtkQue(EnemyBase e)
    {
        toAtkEnemies.Enqueue(e);
    }

    private int frameAtkCount = 5;
    int currCount = 0;
    IEnumerator EnemiesAtk()
    {
        while(true)
        {
            currCount = 0;
            while(toAtkEnemies.Count!=0&&currCount<frameAtkCount)
            {
                var e = toAtkEnemies.Dequeue();
                if (!e.IsAlive()) continue;
                e.AttackPlayer();
                currCount ++;
            }
            yield return null;
        }
    }
}
