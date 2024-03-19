using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_0_Warrior : Player_Area
{
    public GameObject circle;
    private int shield;
    protected override void Awake()
    {
        character.index = 0;
        base.Awake();
    }
    protected override void OnEnterLevelEvent(int l)
    {
        base.OnEnterLevelEvent(l);
    }
    public override void Reset()
    {
        base.Reset();
        circle.SetActive(false);
    }
    public override void StartAttack()
    {
        base.StartAttack();
        StartCoroutine(CreateShield());
    }

    protected override IEnumerator AttackAnim(List<EnemyBase> enemies)
    {
        AttackEnemies(enemies);
        int range = GetAttackRange();
        circle.transform.localScale = new Vector3(range, range, 1);
        circle.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        circle.SetActive(false);
    }
    protected IEnumerator CreateShield()
    {
        while(true)
        {
            int extra= SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 1);
            if (extra == 0) yield return new WaitForSeconds(1f);
            else if(extra == 1)
            {
                shield = 15;
                yield return new WaitForSeconds(20);
            }
            else if(extra==2)
            {
                shield = 10;
                yield return new WaitForSeconds(10);
            }
        }
    }

    public override int GetAttack()
    {
        int atk = base.GetAttack();
        int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 2);
        if (extra == 0) atk = Mathf.CeilToInt(atk * (1.5f));
        if (extra == 1) atk = Mathf.CeilToInt(atk * (2.0f));
        if (extra == 2) atk = Mathf.CeilToInt(atk * (1.0f));

        return atk;
    }

    public override float GetMoneyEfficiency()
    {
        int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 2);
        if(extra == 0) return 0.2f;
        if (extra == 1) return 0.1f;
        if(extra == 2) return 1.0f;
        return 1.0f;
    }
    public override void BeHurt(int attack)
    {
        if(attack<=shield)
        {
            shield -= attack;
            return;
        }
        else if (shield > 0)
        {
            attack -= shield;
            shield = 0;
        }
        base.BeHurt(attack);
    }
}
