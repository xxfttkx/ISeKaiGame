using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Creature
{
    public Enemy enemy;
    public EnemyDataList_SO enemyData;
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
    protected bool IsMoving
    {
        get => isMoveing;
        set
        {
            if(isMoveing!=value)
            {
                isMoveing = value;
                animator?.SetBool("Move", isMoveing);
            }
        }
    }
    private bool isMoveing;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        material = sp.material;
    }
    protected virtual void Start()
    {
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
    internal void SetLevelBonus(float bonus)
    {
        enemy.attack = Mathf.FloorToInt(enemy.attack * bonus);
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
            damage = Mathf.Min(hp, damage);
            HPPanel.Instance.Show(this.transform.position, damage);
            SaveLoadManager.Instance.SetPlayerExtraData(playerIndex, ExtraType.Hurt,damage);
            hp -= damage;
            if (hp <= 0)
            {
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
    public void Release()
    {
        if (!beReleased)
        {
            hp = 0;
            beReleased = true;
            IsMoving = false;
            StartCoroutine(Dead());
            
        }
        
    }
    protected virtual IEnumerator Dead()
    {
        for(float t=0f;t<=1.0f;t+=0.1f)
        {
            float a = Mathf.SmoothStep(0f, 1.01f, t);
            material.SetFloat("_Reslove", a);
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
    public override void Reset()
    {
        base.Reset();
        isBegingRepelled = false;
        enemy = SOManager.Instance.enemyDataList_SO.GetEnemyByIndex(enemy.index);
        hp = enemy.creature.hp;
        //todo del
        hp = enemy.hp;
        maxHp = hp;
        attackRandomTime = 0;
        material.SetFloat("_Red", 0f);
        material.SetFloat("_Reslove", 0f);
        beReleased = false;
        canMove = true;
        player = PlayerManager.Instance.GetPlayerInControl();
        isMoveing = false;
        LevelManager.Instance.AddEnemyNum(this);
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
        return hp;
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
