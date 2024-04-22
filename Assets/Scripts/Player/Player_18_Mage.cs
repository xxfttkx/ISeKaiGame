using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 吟唱时间长 范围= 吟唱时间 伤害*吟唱时间
public class Player_18_Mage : Player_Single
{
    private float lastChantTime;
    public List<Sprite> chantList;
    public SpriteRenderer chantSp;
    public SpriteRenderer atkRangeCircle;
    protected override void Awake()
    {
        character.index = 18;
        chantSp.enabled = false;
        base.Awake();
    }
    protected override IEnumerator Attack()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, GetAttackRange());
            if (e == null)
            {
                yield return null;
                continue;
            }
            yield return AttackAnim(e);
        }

    }
    protected override IEnumerator AttackAnim(EnemyBase e)
    {
        int lastGlobalIndex = e.GetGlobalIndex();
        yield return Chant();
        if(e==null)
        {
            Debug.Log("e==null");
            yield break;
        }
        int newGlobalIndex = e.GetGlobalIndex();
        if (lastGlobalIndex != newGlobalIndex)
        {
            Debug.Log("target was dead。。。");
            yield break;
        }
        if(!e.IsAlive())
        {
            yield break;
        }
        var enemies = Utils.GetNearEnemies(e._pos, lastChantTime);
        if (enemies != null && enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e);
            }
            // anim
            atkRangeCircle.enabled = true;
            StartCoroutine(DelayDisableCircle());
            yield return new WaitForSeconds(GetSkillCD());
        }
    }
    IEnumerator DelayDisableCircle()
    {
        yield return new WaitForSeconds(Settings.circleAnimTime);
        atkRangeCircle.enabled = false;
    }
    IEnumerator Chant()
    {
        lastChantTime = ChantTime;
        chantSp.enabled = true;
        float temp = lastChantTime - 0.1f;
        float delta = temp / chantList.Count;
        int index = 0;
        for (float curr = 0;curr< temp; curr+= delta)
        {
            chantSp.sprite = chantList[index];
            index++;
            yield return new WaitForSeconds(delta);
        }
        yield return new WaitForSeconds(0.1f);
        chantSp.enabled = false;

    }

    private float ChantTime
    {
        get => 1.0f;
    }
    float DamageBonus
    {
        get => lastChantTime + 1.0f;
    }
    public override int GetAttack()
    {
        int a = base.GetAttack();
        a = Mathf.CeilToInt(a * lastChantTime);
        return a;
    }
}
