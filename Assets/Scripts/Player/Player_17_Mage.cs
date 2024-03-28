using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_17_Mage : Player
{
    private Queue<int> ToAtkQueue;
    protected override void Awake()
    {
        character.index = 17;
        ToAtkQueue = new Queue<int>();
        base.Awake();
    }
    public override void Reset()
    {
        base.Reset();
        ToAtkQueue.Clear();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.PlayerHurtPlayerEvent += OnPlayerHurtPlayerEvent;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.PlayerHurtPlayerEvent += OnPlayerHurtPlayerEvent;
    }

    public override void StartAttack()
    {
        base.StartAttack();
        StartCoroutine(Attack());
        StartCoroutine(AttackUseQueue());
    }
    protected virtual IEnumerator Attack()
    {
        while (true)
        {
            if (extras[2] == 0)
            {
                var p = PlayerManager.Instance.GetMaxHpPlayer();
                PlayerManager.Instance.PlayerHurtPlayer(GetPlayerIndex(), p.GetPlayerIndex(), _atk);
                PoolManager.Instance.CreatePlayer17AtkEffect(_pos);
            }
            else if (extras[2] == 1)
            {
                PoolManager.Instance.CreatePlayer17AtkEffect(_pos);
                foreach (var p in PlayerManager.Instance.players)
                    PlayerManager.Instance.PlayerHurtPlayer(GetPlayerIndex(), p.GetPlayerIndex(), _atk);
            }
            else if (extras[2] == 2)
            {
                var e = Utils.GetNearestEnemy(_pos, _range);
                if (e != null)
                {
                    PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e);
                    PoolManager.Instance.CreatePlayer17AtkEffect(e._pos);
                }
                else
                {
                    
                    yield return null;
                    continue;
                }
            }
            yield return new WaitForSeconds(GetSkillCD());
        }

    }
    void OnPlayerHurtPlayerEvent(int _,int __,int num)
    {
        var e = Utils.GetNearestEnemy(_pos, _range);
        if(e!=null)
        {
            TryAtkEnemy(e, num);
        }
        else
        {
            if(extras[1]==1)
            {
                if(ToAtkQueue.Count<100)
                {
                    ToAtkQueue.Enqueue(num);
                }
            }
            else if(extras[1]==2)
            {
                foreach (var p in PlayerManager.Instance.players)
                    PlayerManager.Instance.PlayerHurtPlayer(GetPlayerIndex(), p.GetPlayerIndex(), num);
            }
        }
    }
    IEnumerator AttackUseQueue()
    {
        while(true)
        {
            if(ToAtkQueue.Count>0)
            {
                var e = Utils.GetNearestEnemy(_pos, _range);
                if(e!=null)
                {
                    var atk = ToAtkQueue.Dequeue();
                    TryAtkEnemy(e, atk);
                    yield return new WaitForSeconds(1f);
                }
            }
            else
            {
                if(extras[1]==2)
                {
                    PoolManager.Instance.CreatePlayer17AtkEffect(_pos);
                    foreach (var p in PlayerManager.Instance.players)
                        PlayerManager.Instance.PlayerHurtPlayer(GetPlayerIndex(), p.GetPlayerIndex(), _atk);
                    yield return new WaitForSeconds(1f);
                }
            }
            yield return null;
        }
    }
    void TryAtkEnemy(EnemyBase e,int atk)
    {
        PoolManager.Instance.CreatePlayer17AtkEffect(e._pos);
        //todo atk anim
        if (extras[2] == 1)
        {
            var enemies = Utils.GetNearEnemies(e._pos, 1f);
            if (enemies == null || enemies.Count == 0)
                Debug.Log("enemies == null || enemies.Count == 0");
            else
                foreach(var en in enemies)
                    PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), en, atk);
        }
        else
        {
            PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e, atk);
        }
    }
}
