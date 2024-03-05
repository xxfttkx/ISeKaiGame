using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuangQiu : MonoBehaviour
{
    public Rigidbody2D rb;
    public float velocity = 5;
    public float seekingEnemyInterval = 0.01f;
    private EnemyBase attacker;
    private float atkRange = 0.3f;
    private int atk;// 需获取发出时的快照
    private bool beReleased;
    private SpriteRenderer sp;
    private float size;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        beReleased = false;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
    }
    private void OnExitLevelEvent(int level)
    {
        Release();
    }
    private void Update()
    {
    }
    private void Reset(float size = -1, float speed = -1)
    {
        if (size == -1)
        {
            size = 0.25f;
        }
        sp.size = new Vector2(size, size);
        this.size = size;
        if (speed == -1) velocity = 6f;
        else velocity = speed;
        atkRange = 0.8f;
        beReleased = false;
    }
    public void AttackEnemy(GameObject target,EnemyBase attacker, float size = -1, float speed = -1)
    {
        Reset(size, speed);
        Vector2 dir = target.transform.position - transform.position;
        dir = dir.normalized;
        this.attacker = attacker;
        atk = attacker.GetAttack();
        StartCoroutine(AttackEnemyIE(dir));
    }
    public void AttackEnemy(Vector2 dir, EnemyBase attacker, float size = -1, float speed = -1)
    {
        Reset(size, speed);
        dir = dir.normalized;
        this.attacker = attacker;
        atk = attacker.GetAttack();
        StartCoroutine(AttackEnemyIE(dir));
    }
    IEnumerator AttackEnemyIE(Vector2 direction)
    {
        while (true)
        {
            StartCoroutine(AutoRelease());
            Player p = PlayerManager.Instance.GetPlayerInControl();
            if(Utils.TryAttackPlayer(this.gameObject, p, size/2+atkRange))
            {
                PlayerManager.Instance.EnemyHurtPlayer(null, p, atk);
                Release();
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
        Release();
    }

    private void Release()
    {
        if(!beReleased)
        {
            beReleased = true;
            PoolManager.Instance.ReleaseObj(this.gameObject, 1);
        }
    }
}
