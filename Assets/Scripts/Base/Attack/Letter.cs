using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : PlayerAtk
{
    SpriteRenderer sp;
    protected override void Awake()
    {
        base.Awake();
        velocity = 20;
        poolIndex = 10;
        autoReleaseTime = -1;
        sp = GetComponent<SpriteRenderer>();
    }
    protected override void Reset()
    {
        base.Reset();
        sp.color = Color.white;
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
                break;
            }
            dir = enemy.transform.position - this.transform.position;
            var distance = dir.magnitude;
            if (distance < Settings.hitEnemyDis)
            {
                if(extras[1]==1)
                {
                    //todo effect
                    var l = Utils.GetNearEnemies(_pos, 1f);
                    if (l != null)
                    {
                        foreach (var e in l)
                            PoolManager.Instance.CreateLetterFollowEnemy(e, atk,_pos,extras);
                        Release();
                    }
                }
                else
                {
                    HurtEnemy(enemy);
                }
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
                _pos = _pos + dir * Time.deltaTime * velocity;
            }
            yield return null;
        }
    }
    public void HurtEnemy(EnemyBase e, int atk = -1, List<int> extras = null)
    {
        if (atk == -1) atk = this.atk;
        if (extras != null) this.extras = extras;
        PlayerManager.Instance.PlayerHurtEnemy(10, e, atk);
        sp.color = new Color(1, 1, 1, 0.3f);
        StartCoroutine(DelayRelease());
        StartCoroutine(StealHeart(e));
        StartCoroutine(FollowEnemy(e));
    }
    IEnumerator FollowEnemy(EnemyBase e)
    {
        while (true)
        {
            if (e.IsAlive())
                _pos = e._pos;
            else
                break;
            yield return null;
        }
        Release();
    }
    IEnumerator StealHeart(EnemyBase e)
    {
        if (extras[2] == 2)
        {
            Release();
            yield break;
        }
        float time = extras[1] == 2 ? 0.5f : 1f;
        int a = extras[3] == 2 ? 2 : 1;
        while (true)
        {
            if (!e.IsAlive()) break;
            PlayerManager.Instance.PlayerHurtEnemy(10, e, a);
            yield return new WaitForSeconds(time);
        }
        Release();
    }
    IEnumerator DelayRelease()
    {
        float time = extras[3] == 1 ? 20f : 10f;
        yield return new WaitForSeconds(time);
        Release();
    }
}
