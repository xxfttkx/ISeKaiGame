using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Rigidbody2D rb;
    public float velocity = 10;
    public float SeekingEnemyInterval = 0.02f;
    private bool beReleased;
    private Player player;
    private int atk;
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
    public void AttackEnemy(Player p, EnemyBase enemy)
    {
        player = p;
        atk = p.GetAttack();
        StartCoroutine(Attack(enemy));
    }
    IEnumerator Attack(EnemyBase enemy)
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
                PlayerManager.Instance.PlayerHurtEnemy(player.GetPlayerIndex(), enemy, atk);
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
        TryAttackNextTarget(enemy);
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
            if (distance < 1f)
            {
                PlayerManager.Instance.PlayerHurtPlayer(player.GetPlayerIndex(), p.GetPlayerIndex(), atk);
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
        TryAttackNextEnemy();
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
            PoolManager.Instance.ReleaseObj(this.gameObject, 8);
        }
    }
    void TryAttackNextTarget(EnemyBase enemy)
    {
        var p = PlayerManager.Instance.GetPlayerInControl();
        if (Vector2.SqrMagnitude(p.transform.position - this.transform.position) <= GetSqrRange())
        {
            StartCoroutine(Attack(p));
            return;
        }
        var e = Utils.GetNearestEnemyExcludeE(this.transform.position, GetRange(), enemy);
        if (e != null)
        {
            StartCoroutine(Attack(e));
            return;
        }
        Release();
    }
    void TryAttackNextEnemy()
    {
        var e = Utils.GetNearestEnemy(this.transform.position, GetRange());
        if (e != null)
        {
            StartCoroutine(Attack(e));
            return;
        }
        Release();
    }
    private float GetRange()
    {
        return 3.0f;
    }
    private float GetSqrRange()
    {
        return GetRange()* GetRange();
    }
}
