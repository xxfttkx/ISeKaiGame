using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Creature
{
    public Enemy enemy;
    public EnemyDataList_SO enemyData;
    public Rigidbody2D rb;
    public Vector2 playerPos;
    public Vector2 movementVec2;
    public Player player;
    public bool isBegingRepelled;
    protected SpriteRenderer sp;
    private Material material;
    private Coroutine red;
    private int globalIndex;
    private bool beReleased;
    private Animator animator;
    protected float levelBonus;
    protected bool inAtkAnim;
    protected float _attack
    {
        get => enemy.creature.attack;
    }
    protected float _speed
    {
        get => enemy.creature.speed;
    }
    protected float _attackSpeed
    {
        get => enemy.creature.attackSpeed;
    }
    protected float _attackRange
    {
        get => enemy.creature.attackRange;
    }

    protected bool CanMove
    {
        get => IsAlive() && !isBegingRepelled && !inAtkAnim;
    }
    protected bool CanAttack
    {
        get => player != null && IsAlive() && !isBegingRepelled;
    }
    protected bool IsMoving
    {
        get => isMoving;
        set
        {
            if (isMoving != value)
            {
                isMoving = value;
                animator?.SetBool("Move", isMoving);
            }
        }
    }
    private bool isMoving;

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
    }
    protected virtual void OnEnable()
    {
        Reset();
        StartCoroutine(GetPlayerPosition());
        StartCoroutine(MoveToPlayer());
        StartCoroutine(Attack());
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        EventHandler.ChangePlayerOnTheFieldEvent += OnChangePlayerOnTheFieldEvent;
    }



    protected virtual void OnDisable()
    {
        movementVec2 = Vector2.zero;
        IsMoving = false;
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        EventHandler.ChangePlayerOnTheFieldEvent -= OnChangePlayerOnTheFieldEvent;
        // StopAllCoroutines();
    }
    internal void SetLevelBonus(float bonus)
    {
        levelBonus = bonus;
    }
    protected virtual IEnumerator Attack() { yield break; }
    protected virtual IEnumerator GetPlayerPosition()
    {
        while (true)
        {
            if (player == null)
            {
                yield return null;
                continue;
            }
            Vector2 deltaPos = player.transform.position - this.transform.position;
            movementVec2 = deltaPos.normalized;
/*            Quaternion rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-20, 20));
            movementVec2 = rotation * movementVec2;*/
            yield return new WaitForSeconds(enemy.getPlayerPosTimeDelta);
        }

    }

    public void BeHurt(int damage, int playerIndex = -1)
    {
        if (IsAlive())
        {
            damage = Mathf.Min(hp, damage);
            EventHandler.CallPlayerHurtEnemyEvent(playerIndex, this, damage);
            SaveLoadManager.Instance.SetPlayerExtraData(playerIndex, ExtraType.Hurt, damage);
            hp -= damage;
            if (hp <= 0)
            {
                PlayerManager.Instance.PlayerKillEnemy(playerIndex, this);
                Release();
            }
            else
            {
                if (red != null) StopCoroutine(red);
                red = StartCoroutine(BeRed());
            }
        }
    }
    IEnumerator BeRed()
    {
        for (float red = material.GetFloat("_Red"); red < 1.0f; red += 0.2f)
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
            rb.velocity = Vector2.zero;
            beReleased = true;
            IsMoving = false;
            StartCoroutine(Dead());
        }

    }
    protected virtual IEnumerator Dead()
    {
        for (float t = 0f; t <= 1.0f; t += 0.1f)
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
        for (float t = 0.1f; t < time; t += 0.1f)
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
        LevelManager.Instance.AddEnemyNum(this);
        isBegingRepelled = false;
        enemy = SOManager.Instance.enemyDataList_SO.GetEnemyByIndex(enemy.index);
        sp.sprite = enemy.creature.sprite;
        hp = Mathf.CeilToInt(enemy.creature.hp*levelBonus);
        maxHp = hp;
        material.SetFloat("_Red", 0f);
        material.SetFloat("_Reslove", 0f);
        beReleased = false;
        player = PlayerManager.Instance.GetPlayerInControl();
        isMoving = false;
        inAtkAnim = false;
        rb.velocity = Vector2.zero;
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
    public int GetEnemyIndex()
    {
        return enemy.index;
    }

    public virtual float GetAttackRange()
    {
        return _attackRange * (1 + allBuff.attackRangeBonus) * levelBonus;
    }
    public float GetSqrAttackRange()
    {
        float range = GetAttackRange();
        return range * range;
    }
    public virtual int GetAttack()
    {
        return Mathf.CeilToInt(_attack * (1 + allBuff.attackRangeBonus) * levelBonus);
    }
    public virtual float GetAttackSpeed()
    {
        return (_attackSpeed * (1 + allBuff.attackSpeedBonus) * levelBonus);
    }
    public virtual float GetSkillCD()
    {
        return 10.0f / GetAttackSpeed();
    }
    public virtual float GetSpeed()
    {
        return (_speed * (1 + allBuff.attackSpeedBonus) * levelBonus);
    }
    public virtual int GetHP()
    {
        return hp;
    }
    public float GetPositiveRandom()
    {
        float randomOffset = UnityEngine.Random.Range(0, 10.0f);
        return randomOffset;
    }
    public virtual void AttackPlayer()
    {
        PlayerManager.Instance.EnemyHurtPlayer(this, player);

    }
    void OnChangePlayerOnTheFieldEvent(Player p)
    {
        player = p;
    }
    public float Money
    {
        get => levelBonus * enemy.money;
    }
}
