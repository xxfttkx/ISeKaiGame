using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    public Rigidbody2D rb;
    public float velocity = 30;
    public float SeekingEnemyInterval = 0.05f;
    private bool beReleased;
    private int atk;
    private int count;
    private int extra;
    private int range;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        beReleased = false;
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
    }
    private void OnDisable()
    {
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        StopAllCoroutines();
    }
    public void AttackEnemy(EnemyBase enemy)
    {
        atk = PlayerManager.Instance.GetPlayerAttack(1);
        count = 0;
        range = PlayerManager.Instance.GetPlayerAttackRange(1);
        extra = SaveLoadManager.Instance.GetPlayerExtra(1,1);
        StartCoroutine(AttackEnemyIE(enemy));
    }
    IEnumerator AttackEnemyIE(EnemyBase enemy)
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
    void OnExitLevelEvent(int level)
    {
        Release();
    }
    void Release()
    {
        if (!beReleased)
        {
            beReleased = true;
            PoolManager.Instance.ReleaseObj(this.gameObject, 2);
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
            StartCoroutine(AttackEnemyIE(e));
        }
    }
}
