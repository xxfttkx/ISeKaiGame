using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : PlayerAtk
{
    public Rigidbody2D rb;
    private int count;
    private int extra;
    private int range;
    protected override void Awake()
    {
        base.Awake();
        velocity = 30;
        poolIndex = 2;
        autoReleaseTime = -1;
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void Reset()
    {
        base.Reset();
        count = 0;
    }
    protected override void GetSomeData()
    {
        range = PlayerManager.Instance.GetPlayerAttackRange(1);
        extra = SaveLoadManager.Instance.GetPlayerExtra(1, 1);
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
            if (distance < 1f)
            {
                PlayerManager.Instance.PlayerHurtEnemy(1, enemy, atk);
                DoExtra();
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
                rb.MovePosition(rb.position + dir * Time.deltaTime * velocity);
            }
            yield return null;
        }
    }
    void DoExtra()
    {
        if (extra == 0)
        {
            Release();
        }
        else if (extra == 1)
        {
            atk -= 2;
            if (atk <= 0)
            {
                Release();
            }
            else
            {
                TryAtkEnemy();
            }
        }
        else if (extra == 2)
        {
            if (count >= 2)
            {
                Release();
            }
            else
            {
                count += 1;
                TryAtkEnemy();
            }
        }
    }
    void TryAtkEnemy()
    {
        var e = Utils.GetNearestEnemy(this.transform.position, range);
        if(e==null)
        {
            Release();
        }
        else
        {
            StartCoroutine(AttackEnemy(e));
        }
    }
}
