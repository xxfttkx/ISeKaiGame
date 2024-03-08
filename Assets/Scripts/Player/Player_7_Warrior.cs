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
        base.Reset();
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
                var cd = new WaitForSeconds(10.0f / GetAttackSpeed());
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
        PlayerManager.Instance.PlayerKnockbackEnemy(GetPlayerIndex(), e,GetKnockbackPower());
        PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e);
        yield return new WaitForSeconds(0.2f);
        stand.SetActive(false);
    }
    public override void BeHurt(int attack)
    {
        int count = 0;
        foreach (var p in PlayerManager.Instance.players)
        {
            if(p.IsAlive())
            {
                count++; 
            }
        }
        int atk = attack / count;
        int remain = attack % count;
        foreach (var p in PlayerManager.Instance.players)
        {
            if (!p.IsAlive()) continue;
            if (p.character.index == 7) continue;
            PlayerManager.Instance.PlayerHurtPlayer(GetPlayerIndex(), p.GetPlayerIndex(), atk);
        }
        base.BeHurt(remain+atk);
    }
    private int GetKnockbackPower()
    {
        return 3;
    }
}
