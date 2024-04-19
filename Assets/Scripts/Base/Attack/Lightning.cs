using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : PlayerAtk
{
    public Rigidbody2D rb;
    private int maxCount;
    private int extra1;
    private int extra2;
    private float range;
    Coroutine atkSomeone;
    protected override void Awake()
    {
        base.Awake();
        useRB = false;
        velocity = 20;
        autoReleaseTime = -1;
        poolIndex = 8;
        rb = GetComponent<Rigidbody2D>();
    }
    public override void AttackEnemy(EnemyBase e, Player p)
    {
        player = p;
        playerIndex = p.GetPlayerIndex();
        atk = PlayerManager.Instance.GetPlayerAttack(playerIndex);
        extras = PlayerManager.Instance.GetPlayerExtras(playerIndex);
        GetSomeData();
        atkSomeone = StartCoroutine(AttackEnemy(e));
        StartCoroutine(AutoRelease());
    }
    protected override void GetSomeData()
    {
        range = player._range;
        extra2 = extras[2];
        if (extra2 == 0) maxCount = 10;
        else if (extra2 == 1) maxCount = 8;
        else if (extra2 == 2) maxCount = 20;
        extra1 = extras[1];
    }
    protected override IEnumerator AttackEnemy(EnemyBase enemy)
    {
        yield return null;
        Vector2 dir;
        while (true)
        {
            if (!enemy.IsAlive())
            {
                Release();
                yield break;
            }
            dir = enemy.transform.position - this.transform.position;
            var distance = dir.magnitude;
            if (distance <= Settings.hitPlayerDis)
            {
                rb.velocity = Vector2.zero;
                StartCoroutine(HitEnemy(enemy));
                break;
            }
            else
            {
                dir = dir.normalized;
                float angle = Vector2.Angle(Vector2.up, dir);
                if (dir.x > 0)
                {
                    angle = -angle;
                }
                this.transform.rotation = Quaternion.Euler(0, 0, angle);
                rb.velocity = dir * _velocity;
                yield return null;
            }

        }
    }
    IEnumerator Attack(Player p)
    {
        yield return null;
        Vector2 dir;
        while (true)
        {
            if (p == null || !p.IsAlive())
            {
                Release();
                yield break;
            }
            dir = p.transform.position - this.transform.position;
            var distance = dir.magnitude;
            if (distance <= Settings.hitPlayerDis)
            {
                rb.velocity = Vector2.zero;
                HitPlayer(p);
                break;
            }
            else
            {
                dir = dir.normalized;
                float angle = Vector2.Angle(Vector2.up, dir);
                if (dir.x > 0)
                {
                    angle = -angle;
                }
                this.transform.rotation = Quaternion.Euler(0, 0, angle);
                rb.velocity = dir * _velocity;
            }
            yield return null;
        }
    }
    void TryAttackNextTarget(EnemyBase enemy)
    {
        var e = Utils.GetNearestEnemyExcludeE(this.transform.position, range, enemy);
        var p = PlayerManager.Instance.GetPlayerInControl();
        float sqrP = Vector2.SqrMagnitude(p.transform.position - this.transform.position);
        if (e == null || extra2 == 2)
        {
            if (sqrP <= GetSqrRange())
            {
                atkSomeone = StartCoroutine(Attack(p));
                return;
            }
            else
            {
                Release();
            }
        }
        else
        {
            if (extra2 == 1)
            {
                atkSomeone = StartCoroutine(AttackEnemy(e));
            }
            else
            {
                float sqrE = Vector2.SqrMagnitude(e.transform.position - this.transform.position);
                if (sqrP < sqrE)
                {
                    atkSomeone = StartCoroutine(Attack(p));
                }
                else
                {
                    atkSomeone = StartCoroutine(AttackEnemy(e));
                }
            }
        }
    }
    void TryAttackNextEnemy()
    {
        var e = Utils.GetNearestEnemy(this.transform.position, range);
        if (e != null)
        {
            atkSomeone = StartCoroutine(AttackEnemy(e));
            return;
        }
        Release();
    }
    private float GetSqrRange()
    {
        return range * range;
    }
    private void HitPlayer(Player p)
    {
        if (atkSomeone != null) StopCoroutine(atkSomeone);
        PlayerManager.Instance.PlayerHurtPlayer(player.GetPlayerIndex(), p.GetPlayerIndex(), atk);
        if (extra1 == 1) p.ApplyBuff("player16_extra1", 10, 0, -.5f, 0, 0, 0, ApplyBuffType.NoOverride);
        else if (extra1 == 2) p.ApplyBuff("player16_extra1", 10, 0, .5f, 0, 0, 0, ApplyBuffType.NoOverride);
        maxCount--;
        if (maxCount == 0) Release();
        else TryAttackNextEnemy();

    }
    protected override IEnumerator HitEnemy(EnemyBase e)
    {
        if (atkSomeone != null) StopCoroutine(atkSomeone);
        if (e == null)
        {
            Release();
            yield break;
        }
        if (extra1 == 1) e.ApplyBuff("player16_extra1", 10, 0, -.5f, 0, 0, 0, ApplyBuffType.NoOverride);
        else if (extra1 == 2) e.ApplyBuff("player16_extra1", 10, 0, .5f, 0, 0, 0, ApplyBuffType.NoOverride);
        PlayerManager.Instance.PlayerHurtEnemy(player.GetPlayerIndex(), e, atk);
        maxCount--;
        if (maxCount == 0) Release();
        else TryAttackNextTarget(e);
    }
}
