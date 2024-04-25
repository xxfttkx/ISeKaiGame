using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ÓÂÕß
public class Player_20_Assassin : Player
{
    private int damageNum;
    protected override void Awake()
    {
        character.index = 20;
        base.Awake();
    }
    public override void Reset()
    {
        base.Reset();
        damageNum = 0;
    }
    public override void StartAttack()
    {
        base.StartAttack();
        StartCoroutine(Attack());
        StartCoroutine(AttackEnemy());
    }
    int storageMax
    {
        get => extras[1] switch
        {
            0 => 1000,
            1 => 10000,
            2 => 1000,
            _ => 1000,
        };
    }
    int storageTimes
    {
        get => extras[1] switch
        {
            0 => 1,
            1 => 1,
            2 => 2,
            _ => 1,
        };
    }

    protected IEnumerator Attack()
    {
        while (true)
        {
            var a = _atk;
            foreach (var p in PlayerManager.Instance.players)
            {
                if (!p.IsAlive() || p.GetPlayerIndex() == 20) continue;
                var num = p.GetHp() - 1;
                num = Mathf.Min(a, num);
                PlayerManager.Instance.PlayerHurtPlayer(19, p.GetPlayerIndex(), num);
                num *= storageTimes;
                damageNum = Mathf.Min(storageMax, damageNum + num);
            }
            yield return new WaitForSeconds(GetSkillCD());
        }

    }
    protected IEnumerator AttackEnemy()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, GetAttackRange());
            if (e != null)
            {
                int num = e.GetHP();
                int a = damageNum < num ? damageNum : num;
                PlayerManager.Instance.PlayerHurtEnemy(20, e, a);
                damageNum -= a;
            }
            yield return null;
            continue;
        }

    }
    float AtkBonus
    {
        get => 0.5f;
    }
    float AtkRangeBonus
    {
        get => 0.5f;
    }
    public override void AddBuffBeforeStart()
    {
        foreach (var p in PlayerManager.Instance.players)
        {
            p.RemoveBuff("20_1");
        }
        if (extras[1] == 1)
        {
            foreach (var p in PlayerManager.Instance.players)
            {
                p.ApplyBuff("20_1", -1, -AtkBonus, 0f, 0f, 0f, 0f, ApplyBuffType.Override);
            }
            this.ApplyBuff("20_1", -1, AtkBonus, 0f, 0f, 0f, 0f, ApplyBuffType.Override);
        }
        else if (extras[1] == 2)
        {
            foreach (var p in PlayerManager.Instance.players)
            {
                p.ApplyBuff("20_1", -1, 0f, 0f, 0f, -AtkRangeBonus, 0f, ApplyBuffType.Override);
            }
            this.ApplyBuff("20_1", -1, 0f, 0f, 0f, AtkRangeBonus, 0f, ApplyBuffType.Override);
        }
    }
    protected override void OnDesireChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        base.OnDesireChangeEvent(playerIndex, extraIndex, selectedIndex);
        if (playerIndex == GetPlayerIndex() && (extraIndex == 1))
        {
            AddBuffBeforeStart();
        }
    }
}
