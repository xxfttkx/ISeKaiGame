using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather : MonoBehaviour
{
    public Rigidbody2D rb;
    public float velocity = 30;
    public float SeekingEnemyInterval = 0.05f;
    private bool beReleased;
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
        StartCoroutine(AttackEnemyIE(enemy));
    }
    IEnumerator AttackEnemyIE(EnemyBase enemy)
    {
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
                PlayerManager.Instance.PlayerHurtEnemy(1, enemy);
                Release();
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

        yield break;

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

}
