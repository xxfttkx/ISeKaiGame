using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : MonoBehaviour
{
    protected int poolIndex;
    public bool bReleased = false;
    protected Player player;
    protected int atk;
    protected int playerIndex;
    protected List<int> extras;
    protected float autoReleaseTime = 10f;
    int enemyLayerIndex;
    protected float velocity = 10;
    protected bool useRB = true;
    protected float _velocity
    {
        get => GetProjectileSpeedBonus() * velocity;
    }

    protected virtual void Awake()
    {
        enemyLayerIndex = LayerMask.NameToLayer("Enemy");
    }
    protected Vector3 _localScale
    {
        get => this.transform.localScale;
        set => this.transform.localScale = value;
    }
    protected Vector2 _pos
    {
        get => this.transform.position;
        set => this.transform.position = value;
    }
    protected virtual void OnEnable()
    {
        Reset();
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
    }
    private void OnDisable()
    {
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        StopAllCoroutines();
    }
    protected virtual void Reset()
    {
        bReleased = false;
    }
    void OnExitLevelEvent(int _)
    {
        Release();
    }
    public virtual void AttackEnemy(EnemyBase e, Player p)
    {
        player = p;
        playerIndex = p.GetPlayerIndex();
        atk = PlayerManager.Instance.GetPlayerAttack(playerIndex);
        extras = PlayerManager.Instance.GetPlayerExtras(playerIndex);
        GetSomeData();
        StartCoroutine(AttackEnemy(e));
        StartCoroutine(AutoRelease());
    }
    protected IEnumerator AutoRelease()
    {
        if (autoReleaseTime < 0) yield break;
        yield return new WaitForSeconds(autoReleaseTime);
        Release();
    }
    public virtual void AttackEnemy(Vector2 dir, Player p)
    {
        player = p;
        playerIndex = p.GetPlayerIndex();
        atk = PlayerManager.Instance.GetPlayerAttack(playerIndex);
        extras = PlayerManager.Instance.GetPlayerExtras(playerIndex);
        StartCoroutine(AttackEnemy(dir));
    }
    protected virtual IEnumerator AttackEnemy(EnemyBase e)
    {
        yield break;
    }
    protected virtual IEnumerator AttackEnemy(Vector2 dir)
    {
        yield break;
    }
    public virtual void Release()
    {
        if (bReleased) return;
        bReleased = true;
        StartCoroutine(NextFrameRelease());

    }
    IEnumerator NextFrameRelease()
    {
        yield return null;
        PoolManager.Instance.ReleaseObj(this.gameObject, poolIndex);
    }
    protected virtual void GetSomeData()
    {

    }

    protected float GetProjectileSpeedBonus()
    {
        return player == null ? 1 : player.GetProjectileSpeedBonus();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!useRB) return;
        if (collision.gameObject.layer == enemyLayerIndex)
        {
            var e = collision.gameObject.GetComponent<EnemyBase>();
            if (e != null)
            {
                StartCoroutine(HitEnemy(e));
            }

        }
    }
    protected virtual IEnumerator HitEnemy(EnemyBase e)
    {
        if (e == null) yield break;
    }
}
