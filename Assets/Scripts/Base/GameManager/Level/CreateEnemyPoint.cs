using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemyPoint : MonoBehaviour
{
    public float currTime;
    public float randomPos = 4;
    public int enemyIndex;
    public int firstTime;
    public int deltaTime;
    public int endTime;
    public Player p;
    void OnEnable()
    {
    }

    internal void Reset(int enemyIndex,int firstTime,int deltaTime,int endTime)
    {
        currTime = 0;
        p = PlayerManager.Instance.GetPlayerInControl();
        //camera z ÊÇ -10 ...
        this.enemyIndex = enemyIndex;
        this.firstTime = firstTime;
        this.deltaTime = deltaTime;
        this.endTime = endTime;
        StopAllCoroutines();
        StartCoroutine(CreateEnemies());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator CreateEnemies()
    {
        while (true)
        {
            if (currTime < firstTime)
            {
                yield return new WaitForSeconds(firstTime - currTime);
                currTime = firstTime;
            }
            if (currTime > endTime)
            {
                yield break;
            }
            if (!GameStateManager.Instance.InGamePlay())
            {
                yield return null;
                continue;
            }
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-randomPos, randomPos), Random.Range(-randomPos, randomPos), 0);
            PoolManager.Instance.CreateEnemy(enemyIndex, pos);
            
            yield return new WaitForSeconds(deltaTime);
            currTime += deltaTime;
        }
    }
}