using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : PlayerAtk
{
    public Rigidbody2D rb;
    public float velocity = 10;
    public float SeekingEnemyInterval = 0.02f;
    private int maxCount;
    private int extra1;
    private int extra2;
    private int range;
    private void Awake()
    {
        poolIndex = 8;
        rb = GetComponent<Rigidbody2D>();
    }
    public override void AttackEnemy(EnemyBase e, Player p)
    {
        base.AttackEnemy(e, p);
        range = PlayerManager.Instance.GetPlayerAttackRange(p.GetPlayerIndex());
        extra2 = SaveLoadManager.Instance.GetPlayerExtra(p.GetPlayerIndex(), 2);
        if (extra2 == 0) maxCount = 10;
        else if(extra2 == 1) maxCount = 8;
        else if(extra2 == 2) maxCount = 20;
        extra1 = SaveLoadManager.Instance.GetPlayerExtra(p.GetPlayerIndex(), 1);
    }
    protected override IEnumerator AttackEnemy(EnemyBase enemy)
    {
        yield return null;
        Vector2 dir;
        while (true)
        {
            if(!enemy.IsAlive())
            {
                Release();
                yield break;
            }
            dir = enemy.transform.position - this.transform.position;
            var distance = dir.magnitude;
            if (distance <= Settings.hitPlayerDis)
            {
                HitEnemy(enemy);
                break;
            }
            else
            {
                dir = dir.normalized;
                float angle = Vector2.Angle(Vector2.up, dir);
                if(dir.x >0)
                {
                    angle = -angle;
                }
                this.transform.rotation = Quaternion.Euler(0, 0, angle);
                rb.MovePosition(rb.position + dir * SeekingEnemyInterval * velocity);
            }
            yield return new WaitForSeconds(SeekingEnemyInterval);
        }
    }
    IEnumerator Attack(Player p)
    {
        yield return null;
        Vector2 dir;
        while (true)
        {
            if (!p.IsAlive())
            {
                Release();
                yield break;
            }
            dir = p.transform.position - this.transform.position;
            var distance = dir.magnitude;
            if (distance <= Settings.hitPlayerDis)
            {
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
                rb.MovePosition(rb.position + dir * SeekingEnemyInterval * velocity);
            }
            yield return new WaitForSeconds(SeekingEnemyInterval);
        }
    }
    void TryAttackNextTarget(EnemyBase enemy)
    {
        var e = Utils.GetNearestEnemyExcludeE(this.transform.position, range, enemy);
        var p = PlayerManager.Instance.GetPlayerInControl();
        float sqrP = Vector2.SqrMagnitude(p.transform.position - this.transform.position);
        if (e == null || extra2 == 2)
        {
            if(sqrP<=GetSqrRange())
            {
                StartCoroutine(Attack(p));
                return;
            }
            else
            {
                Release();
            }
        }
        else
        {
            if(extra2==1)
            {
                StartCoroutine(AttackEnemy(e));
            }
            else
            {
                float sqrE = Vector2.SqrMagnitude(e.transform.position - this.transform.position);
                if (sqrP < sqrE)
                {
                    StartCoroutine(Attack(p));
                }
                else
                {
                    StartCoroutine(AttackEnemy(e));
                }
            }
        }
    }
    void TryAttackNextEnemy()
    {
        var e = Utils.GetNearestEnemy(this.transform.position, range);
        if (e != null)
        {
            StartCoroutine(AttackEnemy(e));
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
        PlayerManager.Instance.PlayerHurtPlayer(player.GetPlayerIndex(), p.GetPlayerIndex(), atk);
        maxCount--;
        if(maxCount==0) Release();
        else TryAttackNextEnemy();

    }
    private void HitEnemy(EnemyBase e)
    {
        PlayerManager.Instance.PlayerHurtEnemy(player.GetPlayerIndex(), e, atk);
        maxCount--;
        if (maxCount == 0) Release();
        else TryAttackNextTarget(e);
    }
}
