using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase: MonoBehaviour
{
    public Enemy enemy;
    public EnemyDataList_SO enemyData;
    public Dictionary<string, Buff> buffs;
    private Buff allBuff;
    public Rigidbody2D rb;
    public Coroutine moveToPlayer;
    public Vector2 playerPos;
    public Vector2 movementVec2;
    public Player player;
    public bool isBegingRepelled;
    protected SpriteRenderer sp;
    private Material material;
    public float moveDelta = 0.01f;
    public float attackRandomTime = 0f;
    private Coroutine red;
    private int globalIndex;
    private bool beReleased;
    public bool canMove;
    private Animator animator;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        material = sp.material;
    }
    protected virtual void Start()
    {
        isBegingRepelled = false;
        enemy = SOManager.Instance.enemyDataList_SO.enemies[enemy.index];
        sp.sprite = enemy.sprite;
    }
    protected virtual void Update()
    {

    }
    protected virtual void FixedUpdate()
    {
        
    }
    protected virtual void OnEnable()
    {
        Reset();
        StartCoroutine(GetPlayerPosition());
        StartCoroutine(MoveToPlayer());
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        EventHandler.ChangePlayerOnTheFieldEvent += OnChangePlayerOnTheFieldEvent;
    }
    protected virtual void OnDisable()
    {
        movementVec2 = Vector2.zero;  
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        EventHandler.ChangePlayerOnTheFieldEvent -= OnChangePlayerOnTheFieldEvent;
        StopAllCoroutines();
    }
    protected virtual IEnumerator GetPlayerPosition()
    {
        while (true)
        {
            if(player==null)
            {
                yield return null;
                continue;
            }
            Vector2 deltaPos = player.transform.position - this.transform.position;
            movementVec2 = deltaPos.normalized;
            Quaternion rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-20, 20));
            movementVec2 = rotation * movementVec2;
            yield return new WaitForSeconds(enemy.getPlayerPosTimeDelta);
        }

    }

    public void BeHurt(int damage,int playerIndex = -1)
    {
        if (IsAlive())
        {
            SaveLoadManager.Instance.SetPlayerExtraData(playerIndex, ExtraType.Hurt, Mathf.Min(enemy.hp,damage));
            enemy.hp -= damage;
            if (enemy.hp <= 0)
            {
                //TODO
                PlayerManager.Instance.PlayerKillEnemy(playerIndex, this);
                Release();
            }
            else
            {
                if (red!=null) StopCoroutine(red);
                red = StartCoroutine(BeRed());
            }
        }
    }
    IEnumerator BeRed()
    {
         
        for(float red = material.GetFloat("_Red");red<1.0f;red+=0.2f)
        {
            material.SetFloat("_Red", red);
            yield return new WaitForSeconds(0.02f);
        }
        for (float red = 1.0f; red > 0f; red -= 0.2f)
        {
            yield return new WaitForSeconds(0.02f);
            material.SetFloat("_Red", red);
        }
        material.SetFloat("_Red", 0f);
    }

    public bool IsAlive()
    {
        return enemy.hp > 0;
    }
    public void Release()
    {
        if (!beReleased)
        {
            enemy.hp = 0;
            beReleased = true;
            animator?.SetBool("Dead", true);
            StartCoroutine(Dead());
            
        }
        
    }
    protected virtual IEnumerator Dead()
    {
        float twistAmount = 0f;
        float twistRadius = 0.8f;
        float zoomAmount = 1f;
        
        for(float t=0;t<=1.0f;t+=0.1f)
        {
            twistAmount += 0.15f;
            twistRadius += 0.1f;
            zoomAmount = Mathf.SmoothStep(1, 10, t);
            material.SetFloat("_TwistUvAmount", twistAmount);
            material.SetFloat("_TwistUvRadius", twistRadius);
            material.SetFloat("_ZoomUvAmount", zoomAmount);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        PoolManager.Instance.ReleaseEnemy(this.gameObject, enemy.index);
    }

    protected virtual IEnumerator MoveToPlayer()
    {
        yield break;
    }
    
    
    //±»»÷ÍË
    public void BeRepelled(Player p, float force)
    {
        if (!IsAlive()) return;
        float time = 0.5f;
        Vector2 dir = this.transform.position - p.transform.position;
        dir = dir.normalized;
        StartCoroutine(Repelled(dir, time, force));
    }

    IEnumerator Repelled(Vector3 dir, float time, float force)
    {
        isBegingRepelled = true;
        rb.velocity = dir * force;
        for (float t = 0.1f; t < time; t+=0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            rb.velocity = Vector2.Lerp(dir * force, Vector2.zero, t / time);
        }
        rb.velocity = Vector2.zero;
        isBegingRepelled = false;
    }
    protected virtual void Reset()
    {
        isBegingRepelled = false;
        enemy = SOManager.Instance.enemyDataList_SO.GetEnemyByIndex(enemy.index);
        attackRandomTime = 0;
        material.SetFloat("_Red", 0f);
        material.SetFloat("_TwistUvAmount", 0f);
        material.SetFloat("_TwistUvRadius", 0.8f);
        material.SetFloat("_ZoomUvAmount", 1.0f);
        allBuff = new Buff("all", 0, 0, 0, 0);
        beReleased = false;
        canMove = true;
        player = PlayerManager.Instance.GetPlayerInControl();
        animator?.SetBool("Dead", false);
    }
    private void OnExitLevelEvent(int level)
    {
        Release();
    }

    public void SetGlobalIndex(int index)
    {
        globalIndex = index;
    }
    public int GetGlobalIndex()
    {
        return globalIndex;
    }

    public virtual float GetAttackRange()
    {
        float range = enemy.attackRange;
        range = (range * (1 + allBuff.attackRangeBonus));
        return range;
    }
    public float GetSqrAttackRange()
    {
        float range = GetAttackRange();
        return range * range;
    }
    public virtual int GetAttack()
    {
        int a = enemy.attack;
        a = Mathf.CeilToInt(a * (1 + allBuff.attackBonus));
        return a;
    }
    public virtual float GetAttackSpeed()
    {
        float s = enemy.attackSpeed;
        s = (s * (1 + allBuff.attackSpeedBonus));
        return s;
    }
    public virtual int GetSpeed()
    {
        int s = enemy.speed;
        s = Mathf.CeilToInt(s * (1 + allBuff.speedBonus));
        return s;
    }
    public virtual int GetHP()
    {
        return enemy.hp;
    }
    public float GetRandomOffset()
    {
        float randomOffset = UnityEngine.Random.Range(0, 1.0f);
        if (attackRandomTime > 0)
        {
            attackRandomTime -= randomOffset;
            randomOffset = -randomOffset;
        }
        else
            attackRandomTime += randomOffset;
        return randomOffset;
    }
    protected virtual void AttackPlayer()
    {
        PlayerManager.Instance.EnemyHurtPlayer(this, player);

    }
    void OnChangePlayerOnTheFieldEvent(Player p)
    {
        player = p;
    }
}
