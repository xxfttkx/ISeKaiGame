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
    
    protected int atk
    {
        get => character.creature.attack;
        set => character.creature.attack = value;
    }
    protected int speed
    {
        get => character.creature.speed;
        set => character.creature.speed = value;
    }
    protected int atkSpeed
    {
        get => character.creature.attackSpeed;
        set => character.creature.attackSpeed = value;
    }
    protected int atkRange
    {
        get => character.creature.attackRange;
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
        EventHandler.ExtraChangeEvent += OnExtraChangeEvent;
    }
    protected virtual void OnDisable()
    {
        EventHandler.EnterLevelEvent -= OnEnterLevelEvent;
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        EventHandler.ExtraChangeEvent -= OnExtraChangeEvent;
    }
    public override void Reset()
    {
        base.Reset();
        buffs.Clear();
        allBuff = new Buff("all", 0, 0, 0, 0);
        timeOnTheField = 0;
        character = characterData.GetCharByIndex(character.index);
        GetPlayerDataByPrefession(character.profession);
        hp = character.creature.hp;
        maxHp = hp;
        addHp = 0;
        ChangeCharValByExtra();
    }

    protected virtual void OnEnterLevelEvent(int _)
    {
        // playermanager 中 Reset();
    }
    protected virtual void OnExitLevelEvent(int _)
    {
        StopAllCoroutines();
    }

    public void Move(Vector2 movementInput, float deltaTime)
    {
        if (movementInput.x < 0) sp.flipX = !character.creature.faceToLeft;
        else if (movementInput.x > 0) sp.flipX = character.creature.faceToLeft;
        // rb.MovePosition(rb.position + movementInput * character.speed * deltaTime);
    }
    private void FixedUpdate()
    {
        // if (PlayerManager.Instance.currPlayerIndex == character.index) return;

        // AIControl();
    }

    public virtual void BeHurt(int attack)
    {
        if (hp <= 0) return;
        if (attack == 0) return;
        SaveLoadManager.Instance.SetPlayerExtraData(GetPlayerIndex(), ExtraType.BeHurt, Mathf.Min(hp, attack));
        hp -= attack;
        UIManager.Instance.HPChange(GetPlayerIndex(), GetHpVal());
        if (hp <= 0)
        {
            EventHandler.CallPlayerDeadEvent(GetPlayerIndex());
            PlayerManager.Instance.PlayerDead(GetPlayerIndex());
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
        //todo 能达到的最大hp可不该从so中取。。
        var tempHP = hp+ heal;
        var maxHP = GetMaxHP();
        heal = hp <= maxHP ? heal : maxHP - hp;
        SaveLoadManager.Instance.SetPlayerExtraData(restorer, ExtraType.Heal, heal);
        hp += heal;
        UIManager.Instance.HPChange(character.index, GetHpVal());
    }

    private void AIControl()
    {
        // test...
        Player target = PlayerManager.Instance.GetPlayerInControl();
        if (target == null || !target.IsAlive()) return;
        Vector2 movement = target.transform.position - this.transform.position;
        if (movement.magnitude > GetAttackRange())//todo
        {
            Move(movement.normalized, Time.deltaTime);
        }
        else
        {
            Stop();
        }
        return;
        /*       switch (character.profession)
               {
                   case Profession.Warrior:
                       AIWarrior();
                       break;
                   case Profession.Priest:
                       AIPriest();
                       break;
                   case Profession.Mage:
                       AIMage();
                       break;
                   case Profession.Assassin:
                       AIAssassin();
                       break;
                   default: break;
               }*/
    }
    private void AIWarrior()
    {
    }
    private void AIPriest()
    {
        Player target = PlayerManager.Instance.FindEnemyTarget();
        if (target == null) return;//todo: no warrior
        Vector2 movement = target.transform.position - this.transform.position;
        if (movement.magnitude > 5)//todo
        {
            Move(movement.normalized, 0.02f);
        }
        else
        {
            Stop();
        }

    }
    private void AIMage()
    {
        AIPriest();
    }
    private void AIAssassin()
    {
        AIPriest();
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

    }

    public void BeCompanionHurt(int atk)
    {
        if (!IsAlive())
        {
            Debug.Log("Char hp<0 error...??");
            return;
        }
        atk = Mathf.Min(hp - 1, atk);
        SaveLoadManager.Instance.SetPlayerExtraData(GetPlayerIndex(), ExtraType.BeHurt, atk);
        hp -= atk;
        UIManager.Instance.HPChange(character.index, GetHpVal());
    }


    public virtual void AddBuff(string name, float bonus)
    {
        Buff b = base.ApplyBuffNoOverride(name, -1, bonus, bonus, bonus, bonus, 0f);
        if (b != null)
            UIManager.Instance.BuffChange(GetPlayerIndex(), b);
    }
    public void AddBuff(string name, float atk, float speed, float atkRange, float atkSpeed)
    {
        if (buffs.ContainsKey(name))
        {
            return;
        }
        Buff b = new Buff(name, atk, speed, atkRange, atkSpeed);
        allBuff.AddBuff(b);
        if (b.duration > 0)
        {
            //todo startcourtine
        }
        UIManager.Instance.BuffChange(GetPlayerIndex(), b);
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
        UIManager.Instance.FieldTimeChange(GetPlayerIndex(), timeOnTheField);
    }
    public void SubTimeBonus(float f)
    {
        if (timeOnTheField <= 0) return;
        timeOnTheField -= f;
        if (timeOnTheField < 0) timeOnTheField = 0;
        UIManager.Instance.FieldTimeChange(GetPlayerIndex(), timeOnTheField);
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
        if (extras.Count >= index)
        {
            Debug.Log("extras.Count >= index");
            return 0;
        }
        return extras[index];
    }
    protected virtual void OnExtraChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        if (playerIndex != GetPlayerIndex()) return;
        if (extraIndex == 0)
        {
            int last = extras[extraIndex];
            if (last != selectedIndex)
            {
                character = SOManager.Instance.GetPlayerDataByIndex(GetPlayerIndex());
                if(selectedIndex!=0)
                {
                    var i = selectedIndex - 1==0?1:-1;
                    ChangeCharVal(character.extraCharacteristics[0], i*character.extraCharacteristicVals[0]);
                    ChangeCharVal(character.extraCharacteristics[1], -i*character.extraCharacteristicVals[1]);
                    PlayerSettingsPanel.Instance?.ChangeCh(this);
                }
            }
        }
        extras[extraIndex] = selectedIndex;
    }
    void ChangeCharValByExtra()
    {
        var selectedIndex = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 0);
        if (selectedIndex != 0)
        {
            var i = selectedIndex - 1 == 0 ? 1 : -1;
            ChangeCharVal(character.extraCharacteristics[0], i * character.extraCharacteristicVals[0]);
            ChangeCharVal(character.extraCharacteristics[1], -i * character.extraCharacteristicVals[1]);
            PlayerSettingsPanel.Instance?.ChangeCh(this);
        }
    }
    void ChangeCharVal(Characteristic type,int val)
    {
        //todo...
        _ = type switch
        {
            Characteristic.Hp => hp += val,//todo jjj
            Characteristic.Attack => atk += val,
            Characteristic.Speed => speed += val,
            Characteristic.AttackSpeed => atkSpeed += val,
            Characteristic.AttackRange => atkRange += val,
            _ =>val,
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
    public int GetRawAtk()
    {
        return atk;
    }
    public int GetRawSpeed()
    {
        return speed;
    }
    public int GetRawAtkSpeed()
    {
        return atkSpeed;
    }
    public int GetRawAtkRange()
    {
        return atkRange;
    }
}
