using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Player : Creature
{
    public CharacterDataList_SO characterData;
    public Character character;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private Material material;
    [SerializeField]
    private float timeOnTheField;
    public List<int> extras;
    private List<int> expAddCharacteristics; //readonly


    public int _atk
    {// 游戏中
        get => PlayerManager.Instance.GetPlayerAttack(character.index);
    }
    public float _range
    {// 游戏中
        get => PlayerManager.Instance.GetPlayerAttackRange(character.index);
    }
    protected int atk
    {// 面板
        get => GetRawAtk();
        set => character.creature.attack = value;
    }
    protected int speed
    {// nama
        get => GetRawSpeed();
        set => character.creature.speed = value;
    }
    protected int atkSpeed
    {// nama
        get => GetRawAtkSpeed();
        set => character.creature.attackSpeed = value;
    }
    protected int atkRange
    {// nama
        get => GetRawAtkRange();
        set => character.creature.attackRange = value;
    }

    protected override void Awake()
    {
        base.Awake();
        characterData = SOManager.Instance.characterDataList_SO;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        character = characterData.GetCharByIndex(character.index);
        sp.sprite = character.creature.sprite;
        material = sp.material;
        extras = new List<int>(SaveLoadManager.Instance.GetPlayerExtras(GetPlayerIndex()));
        expAddCharacteristics = SaveLoadManager.Instance.GetPlayerAddCharacteristics(GetPlayerIndex());
    }
    protected virtual void OnEnable()
    {
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        EventHandler.DesireChangeEvent += OnDesireChangeEvent;
    }
    protected virtual void OnDisable()
    {
        EventHandler.EnterLevelEvent -= OnEnterLevelEvent;
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        EventHandler.DesireChangeEvent -= OnDesireChangeEvent;
    }
    public override void Reset()
    {
        base.Reset();
        material.SetFloat("_EffectPercent", 0f);
        buffs.Clear();
        allBuff = new Buff("all", 0, 0, 0, 0);
        timeOnTheField = 0;
        character = characterData.GetCharByIndex(character.index);
        GetPlayerDataByPrefession(character.profession);
        hp = character.creature.hp;
        maxHp = hp;
        AddHpAndLimit(5 * expAddCharacteristics[(int)Characteristic.Hp]);
        ChangeCharValByExtra(0);
        transform.localScale = Vector3.one;
    }

    protected virtual void OnEnterLevelEvent(int _)
    {
        // playermanager 中 Reset();
    }
    protected virtual void OnExitLevelEvent(int _)
    {
        RemoveAllBuff();
        StopAllCoroutines();
    }

    public void Move(Vector2 movementInput)
    {
        if (movementInput.x < 0) sp.flipX = false;
        else if (movementInput.x > 0) sp.flipX = true;
        rb.velocity = movementInput;
    }
    private void FixedUpdate()
    {
        // if (PlayerManager.Instance.currPlayerIndex == character.index) return;

        // AIControl();
    }
    public virtual void BeHurt(int attack, EnemyBase e)
    {
        if (!IsAlive()) return;
        if (attack <= 0) return;
        attack = Mathf.Min(hp, attack);
        EventHandler.CallEnemyHurtPlayerEvent(e, GetPlayerIndex(), attack);
        SaveLoadManager.Instance.SetPlayerExtraData(GetPlayerIndex(), ExtraType.BeHurt, attack);
        hp -= attack;
        EventHandler.CallPlayerHpChangeEvent(GetPlayerIndex(), GetHp(), GetMaxHP());
        if (hp <= 0)
        {
            Dead();
        }
    }


    public void BeHealed(int heal, int restorer)
    {
        if (!IsAlive()) return;
        var tempHP = hp + heal;
        var maxHP = GetMaxHP();
        heal = tempHP <= maxHP ? heal : maxHP - hp;
        if (heal <= 0) return;
        SaveLoadManager.Instance.SetPlayerExtraData(restorer, ExtraType.Heal, heal);
        hp += heal;
        EventHandler.CallPlayerHpChangeEvent(GetPlayerIndex(), GetHp(), GetMaxHP());
    }
    private void Stop()
    {
        rb.velocity = Vector2.zero;
    }
    public void Dead()
    {
        transform.DORotate(new Vector3(0, 0, 360), .5f, RotateMode.FastBeyond360);
        transform.DOScale(0f, .5f).OnComplete(() => PlayerManager.Instance.PlayerDead(GetPlayerIndex()));

    }
    IEnumerator Dissolving()
    {
        float duration = 1f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float a = Mathf.Lerp(0f, 1f, t / duration);
            material.SetFloat("_EffectPercent", a);
            yield return null;
        }
        PlayerManager.Instance.PlayerDead(GetPlayerIndex());

    }

    public virtual void BeCompanionHurt(int atk, int atkIndex)
    {
        if (!IsAlive())
        {
            return;
        }
        atk = Mathf.Min(hp - 1, atk);
        if (atk <= 0) return;
        SaveLoadManager.Instance.SetPlayerExtraData(GetPlayerIndex(), ExtraType.BeHurt, atk);
        hp -= atk;
        EventHandler.CallPlayerHurtPlayerEvent(atkIndex, GetPlayerIndex(), atk);
        EventHandler.CallPlayerHpChangeEvent(GetPlayerIndex(), GetHp(), GetMaxHP());
    }
    public virtual float GetMoneyEfficiency()
    {
        return 1.0f;
    }

    public virtual float GetSkillCD()
    {
        float attackSpeed = PlayerManager.Instance.GetPlayerAttackSpeed(GetPlayerIndex());
        return 10.0f / attackSpeed;
    }

    public virtual int GetAttack()
    {
        if (!IsAlive()) return 1;
        return Mathf.CeilToInt(GetRawAtk() * (1 + allBuff.attackBonus) * GetTimeBonus());
    }
    public virtual int GetSpeed()
    {
        return Mathf.CeilToInt(GetRawSpeed() * (1 + allBuff.speedBonus) * GetTimeBonus());
    }
    public virtual int GetAttackSpeed()
    {
        return Mathf.CeilToInt(atkSpeed * (1 + allBuff.attackSpeedBonus) * GetTimeBonus());
    }

    public virtual int GetAttackRange()
    {
        return Mathf.CeilToInt(atkRange * (1 + allBuff.attackRangeBonus) * GetTimeBonus());
    }

    public override int GetPlayerIndex()
    {
        return character.index;
    }

    private float GetTimeBonus()
    {
        // 0-100  100-0
        float b = Mathf.Clamp(timeOnTheField, 0, 100);
        b *= 0.01f;
        return 1.1f - b;
    }
    public void AddTimeBonus(float f)
    {
        if (timeOnTheField >= 99) return;
        timeOnTheField += f;
        if (timeOnTheField > 99) timeOnTheField = 99;
        EventHandler.CallFieldTimeChangeEvent(GetPlayerIndex(), timeOnTheField);
    }
    public void SubTimeBonus(float f)
    {
        if (timeOnTheField <= 0) return;
        timeOnTheField -= f;
        if (timeOnTheField < 0) timeOnTheField = 0;
        EventHandler.CallFieldTimeChangeEvent(GetPlayerIndex(), timeOnTheField);
    }
    public virtual void AddBuffBeforeStart()
    {

    }
    public bool CanAcceptHurt(int atk)
    {
        return hp > atk;
    }
    public int GetExtra(int index)
    {
        if (extras.Count <= index)
        {
            Debug.Log("extras.Count <= index");
            return 0;
        }
        return extras[index];
    }
    protected virtual void OnDesireChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        selectedIndex++;
        if (playerIndex != GetPlayerIndex()) return;
        int last = extras[extraIndex];
        extras[extraIndex] = selectedIndex;
        if (extraIndex == 0)
        {
            if (last != selectedIndex)
            {
                ChangeCharValByExtra(last);
            }
        }

    }
    void ChangeCharValByExtra(int lastDesire)
    {
        var newDesire = extras[0];
        if (lastDesire != newDesire)
        {
            int mul = lastDesire == 0 || newDesire == 0 ? 1 : 2;
            int max = Mathf.Max(lastDesire, newDesire);
            // 0 1 + 1 0 -   0 2 - 2 0 +   1 2 - 2 1 +
            int sign = (lastDesire < newDesire ? 1 : -1) * (max == 2 ? -1 : 1);
            ChangeCharVal(character.extraCharacteristics[0], mul * sign * character.extraCharacteristicVals[0]);
            ChangeCharVal(character.extraCharacteristics[1], -1 * mul * sign * character.extraCharacteristicVals[1]);
            EventHandler.CallPlayerCharacteristicChangeEvent(this);
        }
    }
    void ChangeCharVal(Characteristic type, int val)
    {
        if (type == Characteristic.Hp)
        {
            AddHpLimit(val);
        }
        else
        {
            _ = type switch
            {
                Characteristic.Attack => atk += val,
                Characteristic.Speed => speed += val,
                Characteristic.AttackSpeed => atkSpeed += val,
                Characteristic.AttackRange => atkRange += val,
                _ => val,
            };
        }

    }
    void GetPlayerDataByPrefession(Profession p)
    {
        var d = SOManager.Instance.GetProfessionDataByProfession(p);
        character.creature.hp = d.hp;
        character.creature.attack = d.attack;
        character.creature.speed = d.speed;
        character.creature.attackSpeed = d.attackSpeed;
        character.creature.attackRange = d.attackRange;
    }
    public void ChangeAttackToNum(int num)
    {
        atk = num;
    }
    public void ChangeAttack(int add)
    {
        atk += add;
    }
    public void ChangeAttack(float bonus)
    {
        atk = Mathf.CeilToInt(atk * bonus);
    }
    public virtual void StartAttack()
    {

    }
    public virtual int GetRawAtk()
    {
        return character.creature.attack + allBuff.atkNum + expAddCharacteristics[(int)Characteristic.Attack];
    }
    public virtual int GetRawSpeed()
    {
        return character.creature.speed + allBuff.speedNum + expAddCharacteristics[(int)Characteristic.Speed];
    }
    public virtual int GetRawAtkSpeed()
    {
        return character.creature.attackSpeed + allBuff.atkSpeedNum + expAddCharacteristics[(int)Characteristic.AttackSpeed];
    }
    public virtual int GetRawAtkRange()
    {
        return character.creature.attackRange + allBuff.atkRangeNum + expAddCharacteristics[(int)Characteristic.AttackRange];
    }
    public int GetProfessionData(Characteristic ch)
    {
        var d = SOManager.Instance.GetProfessionDataByProfession(character.profession);
        return ch switch
        {
            Characteristic.Hp => d.hp,
            Characteristic.Attack => d.attack,
            Characteristic.Speed => d.speed,
            Characteristic.AttackSpeed => d.attackSpeed,
            Characteristic.AttackRange => d.attackRange,
            _ => 0,
        };
    }
    public float GetProjectileSpeedBonus()
    {
        return 1.0f + allBuff.ProjectileSpeedBonus;
    }
    public void EnterField()
    {
        sp.enabled = true;
    }
    public void ExitField()
    {
        sp.enabled = false;
    }
    public float KillNumBonus(int e)
    {
        int num = SaveLoadManager.Instance.GetPlayerKillEnemyNum(GetPlayerIndex(), e);
        return Mathf.Clamp01(num * 1f / 100f) + 1f;
    }
    public void OnSubPlayerCharacteristic(Characteristic ch)
    {
        if (ch == Characteristic.Hp)
        {
            if (IsAlive())
            {
                hp = Mathf.Max(1, hp - 5);
            }
            maxHp -= 5;
            EventHandler.CallPlayerHpChangeEvent(GetPlayerIndex(), GetHp(), GetMaxHP());
        }
    }
    public void OnAddPlayerCharacteristic(Characteristic ch)
    {
        if (ch == Characteristic.Hp)
        {
            if (IsAlive())
            {
                hp += 5;
            }
            maxHp += 5;
            EventHandler.CallPlayerHpChangeEvent(GetPlayerIndex(), GetHp(), GetMaxHP());
        }
    }

}
