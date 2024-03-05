using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemyPoint : MonoBehaviour
{
    public float currTime;
    public float fixedTime;
    public float randomPos = 4;

    public Vector3 deltaCameraDistance;
    public int enemyIndex;
    public int firstTime;
    public int deltaTime;
    // endTime 放到manager中 todo。。
    public int endTime;
    public bool HaveCreated = false;



    private void Start()
    {
        
    }
    void OnEnable()
    {
    }

    internal void Reset(Vector2 offset,int enemyIndex,int firstTime,int deltaTime,int endTime)
    {
        currTime = 0;
        fixedTime = 0;
        //camera z 是 -10 ...
        this.deltaCameraDistance = new Vector3(offset.x,offset.y, 10);
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

    void Update()
    {
        fixedTime += Time.deltaTime;
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
                yield return new WaitForSeconds(0.1f);
                continue;
            }
            this.transform.position = Camera.main.transform.position + deltaCameraDistance;
            Vector3 pos = this.transform.position + new Vector3(Random.Range(0, randomPos), Random.Range(0, randomPos), 0);
            PoolManager.Instance.CreateEnemy(enemyIndex, pos);
            
            yield return new WaitForSeconds(deltaTime);
            currTime += deltaTime;
        }
    }
}