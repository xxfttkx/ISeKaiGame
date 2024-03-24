using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class PoolManager : Singleton<PoolManager>
{

    public List<GameObject> poolPrefabs;
    public List<GameObject> enemyPrefabs;
    public List<ObjectPool<GameObject>> poolList;

    // index to prefab
    public List<ObjectPool<GameObject>> enemyPoolList;
    private Queue<GameObject> soundQueue = new Queue<GameObject>();
    protected override void Awake()
    {
        base.Awake();
        poolList = new List<ObjectPool<GameObject>>();
        enemyPoolList = new List<ObjectPool<GameObject>>();
        CreatePool();
    }
    private void OnEnable()
    {

    }

    

    private void OnDisable()
    {
    }
    private void CreatePool()
    {
        List<GameObject>[] tempPrefabs = { poolPrefabs, enemyPrefabs };
        List<ObjectPool<GameObject>>[] tempList = { poolList, enemyPoolList };
        for(int i = 0;i< tempPrefabs.Length;++i)
        {
            var prefabs = tempPrefabs[i];
            var list = tempList[i];
            foreach (GameObject obj in prefabs)
            {
                Transform parent = new GameObject(obj.name).transform;
                parent.SetParent(transform);

                var newPool = new ObjectPool<GameObject>(
                    () => Instantiate(obj, parent),
                    o => { o.SetActive(true); },
                    o => { o.SetActive(false); },
                    o => { Destroy(o); }
                    );
                list.Add(newPool);
            }
        }
        
    }

    public void ReleaseEnemy(GameObject obj, int index)
    {
        ObjectPool<GameObject> objPool = enemyPoolList[index];
        objPool.Release(obj);
        var e = obj.GetComponent<EnemyBase>();
        LevelManager.Instance.SubEnemyNum(e.GetGlobalIndex());
    }
    public void ReleaseObj(GameObject obj, int index)
    {
        ObjectPool<GameObject> objPool = poolList[index];
        objPool.Release(obj);
    }

    public void CreateBubble(EnemyBase enemy, Vector3 pos, Player p)
    {
        ObjectPool<GameObject> objPool = poolList[0];
        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        Bubble b = obj.GetComponent<Bubble>();
        b.AttackEnemy(enemy, p);
    }
    public void CreateBubble(Vector2 dir, Vector3 pos, Player p)
    {
        ObjectPool<GameObject> objPool = poolList[0];
        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        Bubble b = obj.GetComponent<Bubble>();
        b.AttackEnemy(dir, p);
    }
    public void CreateEnemy(int index, Vector3 pos)
    {
        ObjectPool<GameObject> objPool = enemyPoolList[index];
        GameObject obj = objPool.Get();
        obj.transform.position = pos;
    }
    public void CreateGuangQiu(GameObject target, Vector3 pos, EnemyBase attacker)
    {
        ObjectPool<GameObject> objPool = poolList[1];
        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        GuangQiu b = obj.GetComponent<GuangQiu>();
        b.AttackEnemy(target, attacker);
    }
    public void CreateGuangQiu(Vector2 dir, EnemyBase attacker,float size = -1,float speed = -1)
    {
        ObjectPool<GameObject> objPool = poolList[1];
        GameObject obj = objPool.Get();
        obj.transform.position = attacker.transform.position;
        GuangQiu b = obj.GetComponent<GuangQiu>();
        b.AttackEnemy(dir, attacker, size, speed);
    }
    public void CreateFeather(EnemyBase e, Vector3 pos)
    {
        ObjectPool<GameObject> objPool = poolList[2];
        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        Feather f = obj.GetComponent<Feather>();
        f.AttackEnemy(e,PlayerManager.Instance.GetPlayerByPlayerIndex(1));
    }
    public void ClearPools()
    {
        // todo 写个回调？？ backToTitle的时候pool里的release？？？
        /*foreach(var pool in poolList)
        {
            pool.Clear();
        }
        foreach(var pool in enemyPoolList)
        {
            pool.Clear();
        }*/

    }

    public void ThrowBianPao(EnemyBase e, Player p)
    {
        ObjectPool<GameObject> objPool = poolList[4];
        GameObject obj = objPool.Get();
        obj.transform.position = p.transform.position;
        BianPao b = obj.GetComponent<BianPao>();
        b.AttackEnemy(e,p);
    }
    public void BianPaoExplosion(Vector2 pos)
    {
        ObjectPool<GameObject> objPool = poolList[5];
        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        StartCoroutine(ReleaseRoutine(objPool, obj));
    }

    private IEnumerator ReleaseRoutine(ObjectPool<GameObject> pool, GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        pool.Release(obj);
    }
    internal void PlaySoundEffect(SoundDetails soundDetails)
    {
        var obj = GetSoundPoolObject();
        obj.GetComponent<Sound>().SetSound(soundDetails);
        obj.SetActive(true);
        StartCoroutine(DisableSound(obj, soundDetails.soundClip.length));
    }
    private GameObject GetSoundPoolObject()
    {
        if (soundQueue.Count < 2)
            CreateSoundPool();
        return soundQueue.Dequeue();
    }
    private void CreateSoundPool()
    {
        var parent = new GameObject("Sound").transform;
        parent.SetParent(transform);

        for (int i = 0; i < 20; i++)
        {
            GameObject newObj = Instantiate(poolPrefabs[6], parent);
            newObj.SetActive(false);
            soundQueue.Enqueue(newObj);
        }
    }

    private IEnumerator DisableSound(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
        soundQueue.Enqueue(obj);
    }


    /// <summary>
    /// Player7召唤物
    /// </summary>
    /// <param name="pos"></param>
    public void CreateChicken(Vector2 pos)
    {
        ObjectPool<GameObject> objPool = poolList[7];
        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        Chicken c = obj.GetComponent<Chicken>();
        c.Init();
    }
    public void CreateLightning(Player p,EnemyBase e, Vector3 pos)
    {
        ObjectPool<GameObject> objPool = poolList[8];
        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        Lightning f = obj.GetComponent<Lightning>();
        f.AttackEnemy(e,p);
    }
    public void CreateEgg(Player p, EnemyBase e, Vector3 pos,int a)
    {
        ObjectPool<GameObject> objPool = poolList[9];
        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        Egg f = obj.GetComponent<Egg>();
        f.AttackEnemyOrPlayer(e, p,a);
    }
    public void CreateLetter(Player p, EnemyBase e, Vector3 pos)
    {
        ObjectPool<GameObject> objPool = poolList[10];
        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        PlayerAtk f = obj.GetComponent<PlayerAtk>();
        f.AttackEnemy(e, p);
    }
    public void CreateLetterFollowEnemy(EnemyBase e, int atk, Vector2 pos,List<int>extras)
    {
        ObjectPool<GameObject> objPool = poolList[10];
        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        Letter l = obj.GetComponent<Letter>();
        l.HurtEnemy(e, atk, extras);
    }
}
