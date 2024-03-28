using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        material.SetFloat("_Fade", 1f);
        buffs.Clear();
        allBuff = new Buff("all", 0, 0, 0, 0);
        timeOnTheField = 0;
        character = characterData.GetCharByIndex(character.index);
        GetPlayerDataByPrefession(character.profession);
        hp = character.creature.hp;
        maxHp = hp;
        ChangeCharValByExtra(0);
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
        if (movementInput.x < 0) sp.flipX = !character.creature.faceToLeft;
        else if (movementInput.x > 0) sp.flipX = character.creature.faceToLeft;
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
        EventHandler.CallPlayerHpValChangeEvent(GetPlayerIndex(), GetHpVal());
        if (hp <= 0)
        {
            Dead();
        }
    }

    public int GetHp()
    {
        return hp;
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
        EventHandler.CallPlayerHpValChangeEvent(GetPlayerIndex(), GetHpVal());
    }
    private void Stop()
    {
        rb.velocity = Vector2.zero;
    }
    public void Dead()
    {
        StartCoroutine(Dissolving());
    }
    IEnumerator Dissolving()
    {
        for (float i = 1.0f; i >= -0.1f; i -= 0.1f)
        {
            material.SetFloat("_Fade", i);
            yield return new WaitForSeconds(0.1f);
        }
        EventHandler.CallPlayerDeadEvent(GetPlayerIndex());
        PlayerManager.Instance.PlayerDead(GetPlayerIndex());

    }

    public virtual void BeCompanionHurt(int atk,int atkIndex)
    {
        if (!IsAlive())
        {
            return;
        }
        atk = Mathf.Min(hp - 1, atk);
        SaveLoadManager.Instance.SetPlayerExtraData(GetPlayerIndex(), ExtraType.BeHurt, atk);
        hp -= atk;
        EventHandler.CallPlayerHurtPlayerEvent(atkIndex, GetPlayerIndex(),atk);
        EventHandler.CallPlayerHpValChangeEvent(GetPlayerIndex(), GetHpVal());
    }

    public virtual int GetAttack()
    {
        if (!IsAlive()) return 1;
        return Mathf.CeilToInt(atk * (1 + allBuff.attackBonus) * GetTimeBonus());
    }
    public virtual int GetAttackSpeed()
    {
        return Mathf.CeilToInt(atkSpeed * (1 + allBuff.attackSpeedBonus));
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

    public virtual int GetSpeed()
    {
        return Mathf.CeilToInt(speed * (1 + allBuff.speedBonus));
    }



    public virtual int GetAttackRange()
    {
        return Mathf.CeilToInt(atkRange * GetTimeBonus() * (1 + allBuff.attackRangeBonus));
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

    public virtual float GetAttackBonus()
    {
        return 0;
    }
    public virtual void AddBuffBeforeStart()
    {

    }
    public virtual bool CanSelectExtra(int index)
    {
        return false;
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
        //todo...
        _ = type switch
        {
            Characteristic.Hp => hp += val,//todo jjj
            Characteristic.Attack => atk += val,
            Characteristic.Speed => speed += val,
            Characteristic.AttackSpeed => atkSpeed += val,
            Characteristic.AttackRange => atkRange += val,
            _ => val,
        };
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
        return character.creature.attack + allBuff.atkNum;
    }
    public virtual int GetRawSpeed()
    {
        return character.creature.speed + allBuff.speedNum;
    }
    public virtual int GetRawAtkSpeed()
    {
        return character.creature.attackSpeed + allBuff.atkSpeedNum;
    }
    public virtual int GetRawAtkRange()
    {
        return character.creature.attackRange + allBuff.atkRangeNum;
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
        return 1.0f+allBuff.ProjectileSpeedBonus;
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
        return Mathf.Clamp01(num*1f/100f) + 1f;
    }
}
