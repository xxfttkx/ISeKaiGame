using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_4_Mage : Player_Single
{
    protected override void Awake()
    {
        character.index = 4;
        base.Awake();
    }
    public override void Reset()
    {
        base.Reset();
    }
    protected override IEnumerator AttackAnim(EnemyBase e)
    {
        int extra = extras[1];
        Vector2 dir = e.transform.position - this.transform.position;
        PoolManager.Instance.CreateBubble(dir, this.transform.position, this);
        if (extra == 1)
        {
            StartCoroutine(DelayShoot(dir));
        }
        else if(extra==2)
        {
            PoolManager.Instance.CreateBubble(-dir, this.transform.position, this);
        }
        yield break;
    }
    IEnumerator DelayShoot(Vector2 dir)
    {
        yield return new WaitForSeconds(2f);
        PoolManager.Instance.CreateBubble(dir, this.transform.position, this);
    }
    public override void AddBuffBeforeStart()
    {
        foreach(var p in PlayerManager.Instance.players)
        {
            p.ApplyBuff("player4", -1, 0f, GetBuffBonus(), 0f, 0f, 0f, ApplyBuffType.Override);
        }
        
    }
    private float GetBuffBonus()
    {
        int extra = extras[2];
        if (extra == 0) return 0.1f;
        if (extra == 1) return -0.1f;
        if (extra == 2) return -0.2f;
        return 0f;
    }
    protected override void OnDesireChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        base.OnDesireChangeEvent(playerIndex, extraIndex, selectedIndex);
        if (playerIndex==GetPlayerIndex() &&extraIndex == 2)
        {
            AddBuffBeforeStart();
        }
    }
}
