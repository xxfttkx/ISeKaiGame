using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player3_Knife : MonoBehaviour
{
    private Player p;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private HashSet<int> enemies = new HashSet<int>();
    int enemyLayerIndex;
    int atk;
    bool enable = false;
    bool Enable
    {
        set
        {
            enable = value;
            sp.enabled = value;
        }
        get => enable;
    }
    public Vector2 _pos
    {
        get => this.transform.position;
    }
    private void Awake()
    {
        p = GetComponentInParent<Player>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        enemyLayerIndex = LayerMask.NameToLayer("Enemy");
        Enable = false;
    }
    private void OnEnable()
    {
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
    }
    private void OnDisable()
    {
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
    }
    void OnExitLevelEvent(int _)
    {
        transform.DOKill();
        rb.velocity = Vector2.zero;
    }
    void OnEnterLevelEvent(int _)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        Enable = false;
    }
    public IEnumerator AttackDir(Vector2 dir, float range, float time)
    {
        transform.localPosition = Vector3.zero;
        Enable = true;
        atk = p._atk;
        if(p.GetExtra(1)==2)
        {
            atk = Mathf.CeilToInt(atk * Mathf.Lerp(2, 1, (range) / p._range));
        }
        float half = time / 2;
        Vector2 dis = dir * range;
        Vector2 v = dis / half;
        rb.velocity = v;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        yield return new WaitForSeconds(half);
        rb.velocity = Vector2.zero;
        transform.DOLocalMove(Vector2.zero, half);
        dir = -transform.localPosition;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.DOLocalRotate(new Vector3(0, 0, angle), half);
        yield return new WaitForSeconds(half);
        enemies.Clear();
        Enable = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enable) return;
        if (collision.gameObject.layer == enemyLayerIndex)
        {
            var e = collision.gameObject.GetComponent<EnemyBase>();
            if (e != null)
            {
                int i = e.GetGlobalIndex();
                if (!enemies.Contains(i))
                {
                    enemies.Add(i);
                    PlayerManager.Instance.PlayerHurtEnemy(p.GetPlayerIndex(), e, atk);
                }
            }

        }
    }
}
