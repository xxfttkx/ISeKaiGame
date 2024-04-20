using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : PlayerAtk
{
    private Rigidbody2D rb;
    public SpriteRenderer egg;
    public SpriteRenderer breakEgg;
    float range = 1f;
    protected override void Awake()
    {
        base.Awake();
        poolIndex = 9;
        velocity = 30;
        autoReleaseTime = -1;
        useRB = false;
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void Reset()
    {
        base.Reset();
        breakEgg.enabled = false;
        egg.enabled = true;
        _localScale = new Vector3(1f, 1f, 1f);
    }
    public void AttackEnemyOrPlayer(EnemyBase e, Player p, int a)
    {
        atk = a;
        GetSomeData();
        StartCoroutine(AttackEnemyOrPlayer(e,p));
        StartCoroutine(AutoRelease());
    }
    protected IEnumerator AttackEnemyOrPlayer(EnemyBase e, Player p)
    {
        yield return null;
        Vector2 dir;
        Creature target = e != null ? e : p;
        while (true)
        {
            if (!target.IsAlive())
            {
                Release();
                break;
            }
            dir = target.transform.position - this.transform.position;
            var distance = dir.sqrMagnitude;
            if (distance < Settings.hitPlayerDisSqr)
            {
                rb.velocity = Vector2.zero;
                if (target.IsPlayer())
                    PlayerManager.Instance.PlayerHealPlayer(6, target.GetPlayerIndex(), atk);
                else
                    yield return StartCoroutine(AtkEnemiesAnim());
                Release();
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
    IEnumerator AtkEnemiesAnim()
    {
        breakEgg.enabled = true;
        egg.enabled = false;
        float tarS = range * 2;
        float currS = 0f;
        _localScale = new Vector3(currS, currS, 1f);
        while (currS < tarS)
        {
            currS += 0.1f;
            _localScale = new Vector3(currS, currS, 1f);
            yield return new WaitForSeconds(0.01f);
        }
        var enemies = Utils.GetNearEnemies(_pos, range);
        if(enemies!=null)
        {
            foreach (var e in enemies)
            {
                PlayerManager.Instance.PlayerHurtEnemy(6, e, atk);
            }
        }
    }
}
