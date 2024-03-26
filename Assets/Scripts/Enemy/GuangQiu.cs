using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuangQiu : MonoBehaviour
{
    public Rigidbody2D rb;
    public float velocity = 5;
    private EnemyBase attacker;
    private float atkRange = 0.3f;
    private int atk;// 需获取发出时的快照
    private bool beReleased;
    private SpriteRenderer sp;
    private float size;
    int playerLayerIndex;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        playerLayerIndex = LayerMask.NameToLayer("Player");
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
    private void Reset(float size = -1, float speed = -1)
    {
        if (size == -1) size = 0.25f;
        sp.size = new Vector2(size, size);
        this.size = size;
        if (speed == -1) velocity = 6f;
        else velocity = speed;
        beReleased = false;
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
        StartCoroutine(AutoRelease());
        while (true)
        {
            rb.MovePosition(rb.position + direction * Time.deltaTime * velocity);
            yield return null;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayerIndex)
        {
            var p = PlayerManager.Instance.GetPlayerInControl();
            PlayerManager.Instance.EnemyHurtPlayer(null, p, atk);
            Release();
        }
    }
}
