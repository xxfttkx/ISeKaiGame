using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterDataList_SO characterData;
    public Character character;
    public Dictionary<string,Buff> buffs;
    private Buff allBuff;
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private Material material;
    [SerializeField]
    private float timeOnTheField;
    protected virtual void Awake()
    {
        characterData = SOManager.Instance.characterDataList_SO;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        character = characterData.GetCharByIndex(character.index);
        sp.sprite = character.sprite;
        material = sp.material;
        buffs = new Dictionary<string, Buff>();
    }
    protected virtual void OnEnable()
    {
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
    }
    protected virtual void OnDisable()
    {
        EventHandler.EnterLevelEvent -= OnEnterLevelEvent;
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
    }
    public virtual void Reset()
    {
        buffs.Clear();
        allBuff = new Buff("all", 0, 0, 0, 0);
        timeOnTheField = 0;
        character = characterData.GetCharByIndex(character.index);
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
        if (movementInput.x < 0) sp.flipX = !character.faceToLeft;
        else if (movementInput.x > 0) sp.flipX = character.faceToLeft;
        // rb.MovePosition(rb.position + movementInput * character.speed * deltaTime);
    }
    private void FixedUpdate()
    {
        // if (PlayerManager.Instance.currPlayerIndex == character.index) return;

        // AIControl();
    }

    public virtual void BeHurt(int attack)
    {
        if (character.hp <= 0) return;
        if (attack == 0) return;
        SaveLoadManager.Instance.SetPlayerExtraData(GetPlayerIndex(), ExtraType.BeHurt, Mathf.Min(character.hp, attack));
        character.hp -= attack;
        UIManager.Instance.HPChange(GetPlayerIndex(), GetHpVal());
        if (character.hp<=0)
        {
            EventHandler.CallPlayerDeadEvent(character.index);
            PlayerManager.Instance.PlayerDead(character.index);
            Dead();
        }
    }
    public float GetHpVal()
    {
        float val = ((float)character.hp) / GetMaxHP();
        val = Mathf.Clamp01(val);
        return val;
    }
    public void BeHealed(int heal,int restorer)
    {
        if (character.hp <= 0) return;
        //todo 能达到的最大hp可不该从so中取。。
        var tempHP = character.hp + heal;
        var maxHP = GetMaxHP();
        heal = tempHP <= maxHP ? heal : maxHP - character.hp;
        SaveLoadManager.Instance.SetPlayerExtraData(restorer, ExtraType.Heal, heal);
        character.hp += heal;
        UIManager.Instance.HPChange(character.index, GetHpVal());
    }

    private void AIControl()
    {
        // test...
        Player target = PlayerManager.Instance.GetPlayerInControl();
        if (target == null|| !target.IsAlive()) return;
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
        if(movement.magnitude>5)//todo
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
    public bool IsDead()
    {
        return character.hp <= 0;
    }
    public bool IsAlive()
    {
        return character.hp > 0;
    }

    public void BeCompanionHurt(int atk)
    {
        if (character.hp <= 0)
        {
            Debug.Log("Char hp<0 error...??");
            return;
        }
        SaveLoadManager.Instance.SetPlayerExtraData(GetPlayerIndex(), ExtraType.BeHurt, Mathf.Min(character.hp - 1, atk));
        if (character.hp <= atk)
        {
            character.hp = 1;
        }
        else
        {
            character.hp -= atk;
        }
        UIManager.Instance.HPChange(character.index, GetHpVal());
    }


    public virtual void AddBuff(string name,float bonus)
    {
        allBuff.AddBuff(bonus);
        Buff b;
        if (buffs.TryGetValue(name,out b))
        {
            b.AddBuff(bonus);
        }
        else
        {
            b = new Buff(name, bonus, bonus, bonus, bonus);
            buffs.Add(name, b);
        }
        UIManager.Instance.BuffChange(GetPlayerIndex(),b);
    }
    public void AddBuff(string name, float atk, float speed, float atkRange, float atkSpeed)
    {
        if(buffs.ContainsKey(name))
        {
            return;
        }
        Buff b = new Buff(name, atk, speed, atkRange, atkSpeed);
        allBuff.AddBuff(b);
        if(b.duration>0)
        {
            //todo startcourtine
        }
        UIManager.Instance.BuffChange(GetPlayerIndex(), b);
    }

    public virtual int GetAttack()
    {
        if (!IsAlive()) return 1;
        int atk = character.attack;
        atk = Mathf.CeilToInt(atk * (1 + allBuff.attackBonus));
        atk = Mathf.CeilToInt(atk * GetTimeBonus());
        return atk;
    }
    public virtual int GetAttackSpeed()
    {
        int a = character.attackSpeed;
        a = Mathf.CeilToInt(a * (1 + allBuff.attackSpeedBonus));
        return a;
    }

    public virtual float GetMoneyEfficiency()
    {
        return 1.0f;
    }

    public virtual float GetSkillCD()
    {
        float attackSpeed = GetAttackSpeed();
        float cd = 10.0f / attackSpeed;
        return cd;
    }

    public virtual int GetSpeed()
    {
        int atk = character.speed;
        atk = Mathf.CeilToInt(atk * (1 + allBuff.speedBonus));
        return atk;
    }

    public int GetMaxHP()
    {
        int maxHP = SOManager.Instance.characterDataList_SO.GetCharByIndex(character.index).hp;
        maxHP = Mathf.CeilToInt(maxHP * (1 + allBuff.hpBonus));
        return maxHP;
    }

    //todo 存起来 ？？？
    public virtual int GetAttackRange()
    {
        int range = character.attackRange;
        range = Mathf.CeilToInt(range * (1 + allBuff.attackRangeBonus));
        range = Mathf.CeilToInt(range * GetTimeBonus());
        return range;
    }

    public int GetPlayerIndex()
    {
        return character.index;
    }

    private float GetTimeBonus()
    {
        // 0-99  100-1
        float b = Mathf.Clamp(timeOnTheField, 0, 99);
        b *= 0.01f;
        return 1 - b;
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
        return character.hp > atk;
    }
}
