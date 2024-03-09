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
        int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 0);
        if (extra == 1)
        {
            character.attack += 2;
            character.attackRange -= 1;
        }
        else if (extra == 2)
        {
            character.attack += 1;
            character.attackRange -= 2;
        }
    }
    protected override IEnumerator AttackAnim(EnemyBase e)
    {
        int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 1);
        Vector2 dir = e.transform.position - this.transform.position;
        PoolManager.Instance.CreateBubble(dir, this.transform.position, this);
        Debug.Log("hit");
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
        float b = GetBuffBonus();
        this.AddBuff("player3", 0, b, 0, 0);
    }
    private float GetBuffBonus()
    {
        int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 2);
        if (extra == 0) return 0.1f;
        if (extra == 1) return -0.1f;
        if (extra == 2) return -0.2f;
        return 0f;
    }
}
