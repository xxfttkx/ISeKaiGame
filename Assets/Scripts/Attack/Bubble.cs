using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Rigidbody2D rb;
    private float maxDistance;
    public float velocity = 10;
    public float seekingEnemyInterval = 0.05f;
    public float bubbleSmallRadius = 0.5f;
    public float bubbleBigRadius = 1.5f;
    private int atk;
    private int playerIndex;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        Reset();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void AttackEnemy(EnemyBase enemy, Player p)
    {
        atk = p.GetAttack();
        playerIndex = p.GetPlayerIndex();
        Vector2 dir = enemy.transform.position - p.transform.position;
        dir = dir.normalized;
        StartCoroutine(AttackEnemyIE(dir));
    }

    IEnumerator AttackEnemyIE(Vector2 direction)
    {
        while (true)
        {
            StartCoroutine(AutoRelease());
            var enemies = Utils.GetNearEnemies(this.transform.position, bubbleSmallRadius);
            if (enemies!=null&& enemies.Count>0)
            {
                yield return BubuleBigger();
                enemies = Utils.GetNearEnemies(this.transform.position, bubbleBigRadius);
                if(enemies != null && enemies.Count > 0)
                {
                    foreach (var e in enemies)
                    {
                        PlayerManager.Instance.PlayerHurtEnemy(playerIndex, e, atk);
                    }
                }
                
                PoolManager.Instance.ReleaseObj(this.gameObject, 0);
                break;
            }
            else
            {
                rb.MovePosition(rb.position + direction * seekingEnemyInterval * velocity);
            }
            yield return new WaitForSeconds(seekingEnemyInterval);
        }
    }


    IEnumerator AutoRelease()
    {
        yield return new WaitForSeconds(Settings.guangQiuExistTime);
        PoolManager.Instance.ReleaseObj(this.gameObject, 0);
    }

    IEnumerator BubuleBigger()
    {
        for (int i = 1; i <= 3; ++i)
        {
            this.transform.localScale = new Vector3(i, i, 1);
            yield return new WaitForSeconds(0.025f);
        }
    }

    private void Reset()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
    }
}
