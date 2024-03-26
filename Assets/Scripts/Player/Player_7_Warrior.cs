using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_7_Warrior : Player
{
    public GameObject stand;
    protected override void Awake()
    {
        character.index = 7;
        base.Awake();
        stand.SetActive(false);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    public override void Reset()
    {
        stand.SetActive(false);
        base.Reset();
        StartCoroutine(Attack());
    }
    public override void StartAttack()
    {
        base.StartAttack();
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(stand.transform.position, this.GetAttackRange());
            if (e != null)
            {
                StartCoroutine(AttackAnim(e));
                var cd = new WaitForSeconds(GetSkillCD());
                yield return cd;
            }
            else
            {
                yield return null;
            }
        }

    }
    IEnumerator AttackAnim(EnemyBase e)
    {
        stand.SetActive(true);
        PlayerManager.Instance.PlayerKnockbackEnemy(GetPlayerIndex(), e, GetKnockbackPower());
        PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e);
        if (extras[1] == 2)
        {
            var l = Utils.GetSectorEnemies(_pos, e._pos - _pos, GetSectorAngle(), _range);
            if (l != null && l.Count > 0)
            {
                foreach (var ene in l)
                {
                    PlayerManager.Instance.PlayerKnockbackEnemy(GetPlayerIndex(), ene, GetKnockbackPower());
                    PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), ene);
                }
            }
        }
        yield return new WaitForSeconds(0.2f);
        stand.SetActive(false);
    }
    public override void BeHurt(int attack,EnemyBase e)
    {
        if (extras[2] == 0)
        {
            int count = 0;
            foreach (var p in PlayerManager.Instance.players)
            {
                if (p.IsAlive())
                {
                    count++;
                }
            }
            int atk = Mathf.FloorToInt(attack / count);
            int remain = attack % count;
            foreach (var p in PlayerManager.Instance.players)
            {
                if (!p.IsAlive()) continue;
                if (p.character.index == 7) continue;
                if (p.CanAcceptHurt(attack))
                {
                    PlayerManager.Instance.EnemyHurtPlayer(e, p, attack);
                }
                else
                {
                    var a = p.GetHp() - 1;
                    PlayerManager.Instance.EnemyHurtPlayer(e, p, a);
                    remain += (atk - a);
                }
            }
            base.BeHurt(remain + atk,e);
        }
        else
        {
            Player p = null;
            if (extras[2] == 1)
                p = PlayerManager.Instance.GetMaxHpValPlayer();
            else if (extras[2] == 2)
            {
                p = PlayerManager.Instance.GetMinHpValPlayer();
            }
            if (p == this || p == null)
            {
                base.BeHurt(attack,e);
            }
            else
            {
                if (p.CanAcceptHurt(attack))
                {
                    PlayerManager.Instance.EnemyHurtPlayer(null, p, attack);
                }
                else
                {
                    var a = p.GetHp() - 1;
                    PlayerManager.Instance.EnemyHurtPlayer(null, p, a);
                    base.BeHurt(atk - a,e);
                }
            }
        }

    }
    private int GetKnockbackPower()
    {
        return extras[1] switch
        {
            0 => 3,
            1 => 4,
            2 => 2,
            _ => 3,
        };
    }
    float GetSectorAngle()
    {
        return 30;
    }
}
