using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 吟唱时间长 范围 = 吟唱时间 伤害*=吟唱时间
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
                damageNum = Mathf.Min(1000, damageNum + num);
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
}
