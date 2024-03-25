using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : PlayerAtk
{
    public Rigidbody2D rb;
    public float velocity = 10;
    public float seekingEnemyInterval = 0.05f;
    public float bubbleSmallRadius = 0.5f;
    public float bubbleBigRadius = 1.5f;
    float _velocity
    {
        get => GetProjectileSpeedBonus() * velocity;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        poolIndex = 0;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Reset();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected override IEnumerator AttackEnemy(EnemyBase e)
    {
        Vector2 dir = e.transform.position - this.transform.position;
        StartCoroutine(AttackEnemy(dir));
        yield break;
    }
    protected override IEnumerator AttackEnemy(Vector2 dir)
    {
        dir = dir.normalized;
        StartCoroutine(AutoRelease());
        while (true)
        {
            var tempE = Utils.GetNearestEnemy(this.transform.position, bubbleSmallRadius);
            if (tempE != null)
            {
                yield return BubuleBigger();
                var enemies = Utils.GetNearEnemies(this.transform.position, bubbleBigRadius);
                if (enemies != null && enemies.Count > 0)
                {
                    int extra = extras[1];
                    if (extra == 0)
                        atk = Mathf.CeilToInt(atk * 1.0f / enemies.Count);
                    foreach (var enemy in enemies)
                    {
                        PlayerManager.Instance.PlayerHurtEnemy(playerIndex, enemy, atk);
                        if (extra == 2) atk += 1;
                    }
                }
                break;
            }
            else
            {
                rb.MovePosition(rb.position + dir * Time.deltaTime * _velocity);
            }
            yield return null;
        }
        Release();
    }

    IEnumerator BubuleBigger()
    {
        for (int i = 1; i <= 3; ++i)
        {
            this.transform.localScale = new Vector3(i, i, 1);
            yield return new WaitForSeconds(0.025f);
        }
        
    }
    protected override void Reset()
    {
        base.Reset();
        _localScale = new Vector3(1, 1, 1);
    }
}
