using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_5_Mage : Player
{
    public Transform laser;
    public SpriteRenderer laserSp;
    private int targetIndex;
    private float hitTargetTime;
    protected override void Awake()
    {
        character.index = 5;
        base.Awake();
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
        laserSp.size = new Vector2(0, laserSp.size.y);
        
    }
    public override void StartAttack()
    {
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, this.GetAttackRange());
            if (e != null)
            {
                int extra = extras[1];
                if (extra == 0||extra==1)
                {
                    // 可以分开 。。现在直接切换会享受最高加成。。
                    yield return AttackEnemy(e);
                }
                else if(extra==2)
                {
                    ClearEnemyAndTime();
                    yield return AttackEnemyExtra2(e);
                }
                               
                yield return null;
            }
            else
            {
                laserSp.size = new Vector2(0, laserSp.size.y);
                yield return null;
            }
        }
    }
    IEnumerator AttackEnemy(EnemyBase e)
    {
        if(e.GetGlobalIndex()!=targetIndex)
        {
            ClearEnemyAndTime();
        }
        float cd = 10.0f / GetAttackSpeed();
        int atk = PlayerManager.Instance.GetPlayerAttack(5);
        int count = Mathf.CeilToInt(cd / 0.01f);
        int time = Mathf.CeilToInt(count*1.0f / atk);
        
        for (int i = 1; i <= count; ++i)
        {
            Vector2 dir = e.transform.position - this.transform.position;
            var dis = dir.magnitude;
            if (dis > GetAttackRange()) break;
            laserSp.size = new Vector2(dis, laserSp.size.y);
            // 获取角度（弧度）
            float angle = Mathf.Atan2(dir.y, dir.x);
            // 将角度转换为度数
            float angleInDegrees = angle * Mathf.Rad2Deg;
            // 创建一个新的旋转Quaternion
            Quaternion newRotation = Quaternion.Euler(0f, 0f, angleInDegrees);
            // 将新的旋转应用于Transform
            laser.rotation = newRotation;
            if (i % time == 0)
            {
                PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e, 1);
                if (!e.IsAlive())
                {
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        laserSp.size = new Vector2(0, laserSp.size.y);
        hitTargetTime += cd;
    }
    IEnumerator AttackEnemyExtra2(EnemyBase e)
    {
        float cd = 10.0f / GetAttackSpeed();
        int atk = PlayerManager.Instance.GetPlayerAttack(5);
        int count = Mathf.CeilToInt(cd / 0.01f);
        int time = Mathf.CeilToInt(count * 1.0f / atk);
        for (int i = 1; i <= count; ++i)
        {
            Vector2 dir = e.transform.position - this.transform.position;
            var dis = dir.magnitude;
            int range = GetAttackRange();
            if (dis > range) break;

            // 获取角度（弧度）
            float angle = Mathf.Atan2(dir.y, dir.x);
            // 将角度转换为度数
            float angleInDegrees = angle * Mathf.Rad2Deg;
            // 创建一个新的旋转Quaternion
            Quaternion newRotation = Quaternion.Euler(0f, 0f, angleInDegrees);
            // 将新的旋转应用于Transform
            laser.rotation = newRotation;

            laserSp.size = new Vector2(range, laserSp.size.y);
            if (i % time == 0)
            {
                var enimies = Utils.GetEnemiesByDirAndRange(this.transform.position, dir, range);
                if (enimies != null && enimies.Count > 0)
                {
                    foreach (var enemy in enimies)
                    {
                        PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), enemy, 1);
                    }
                }


                if (!e.IsAlive())
                {
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        

    }
    public override int GetAttack()
    {
        float bonus = 1;
        int extra = extras[2];
        if (extra == 0) bonus = Mathf.Clamp01(1.5f - GetHpVal());
        else if (extra == 1) bonus = 1f;
        else if (extra == 2) bonus = GetHp() == 1 ? 2f : 0.5f;
        extra = extras[1];
        if(extra==1)
        {
            bonus *= Mathf.Lerp(1f, 3f, hitTargetTime / 3);
        }
        return Mathf.CeilToInt(base.GetAttack() * bonus);
    }

    void ClearEnemyAndTime()
    {
        targetIndex = -1;
        hitTargetTime = 0f;
    }
    public override void AddBuffBeforeStart()
    {
        foreach (var p in PlayerManager.Instance.players)
        {
            p.ApplyBuff("player5", -1, GetBuffBonus(), 0f,  0f, 0f, 0f, ApplyBuffType.Override);
        }

    }
    private float GetBuffBonus()
    {
        int extra = extras[2];
        if (extra == 0) return 0.2f;
        if (extra == 1) return 0.1f;
        if (extra == 2) return 0.3f;
        return 0f;
    }
    protected override void OnDesireChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        base.OnDesireChangeEvent(playerIndex, extraIndex, selectedIndex);
        if (playerIndex == GetPlayerIndex() && extraIndex == 2)
        {
            AddBuffBeforeStart();
        }
    }
}
