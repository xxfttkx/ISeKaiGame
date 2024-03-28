using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public int hp;
    public int maxHp;
    public int addHp;


    protected Dictionary<string, Buff> buffs;
    protected Dictionary<Buff, Coroutine> buffToRel;
    [SerializeField]
    protected Buff allBuff;
    public Vector2 _pos
    {
        get => this.transform.position;
    }

    protected virtual void Awake()
    {
        buffs = new Dictionary<string, Buff>();
        buffToRel = new Dictionary<Buff, Coroutine>();
        allBuff = new Buff("allBuff");
    }
    public virtual void Reset()
    {
        buffToRel.Clear();
        buffs.Clear();
        allBuff.Clear();
        addHp = 0;
        // hp在子类中。。
    }
    public virtual bool IsAlive()
    {
        return hp > 0;
    }
    public virtual void Behurt(int atk)
    {
        if (IsAlive())
        {
            hp -= atk;
            if (!IsAlive())
            {
                //
            }
        }
    }
    public int GetMaxHP()
    {
        return Mathf.CeilToInt((maxHp) * (1 + allBuff.hpBonus) + addHp);
    }
    public float GetHpVal()
    {
        float val = ((float)hp) / GetMaxHP();
        val = Mathf.Clamp01(val);
        return val;
    }
    public Buff AddBuff(string name, float duration, float atk, float speed, float atkRange, float atkSpeed, float hpB)
    {
        Buff b = new Buff(name, atk, speed, atkRange, atkSpeed);
        buffs.Add(name, b);
        allBuff.AddBuff(b);
        int changeHp = Mathf.CeilToInt(maxHp * hpB);
        if(IsAlive())
        {
            this.hp += changeHp;
            EventHandler.CallPlayerHpValChangeEvent(GetPlayerIndex(), GetHpVal());
        }
        if (duration > 0)
        {
            buffToRel.Add(b,StartCoroutine(BuffTimeCountdown(duration,b)));
        }
        return b;
    }
    public Buff AddBuff(string name, float duration, Characteristic ch, int val, ApplyBuffType type)
    {
        Buff b = new Buff(name, ch, val);
        buffs.Add(name, b);
        allBuff.AddBuff(b);
        int changeHp = b.hpNum;
        if (IsAlive() && changeHp > 0)
        {
            this.hp += changeHp;
            addHp += changeHp;
            EventHandler.CallPlayerHpValChangeEvent(GetPlayerIndex(), GetHpVal());
        }
        if (duration > 0)
        {
            buffToRel.Add(b, StartCoroutine(BuffTimeCountdown(duration, b)));
        }
        return b;
    }
    public Buff AddBuff(string name, float duration, Characteristic ch, float val, ApplyBuffType type)
    {
        Buff b = new Buff(name, ch, val);
        buffs.Add(name, b);
        allBuff.AddBuff(b);
        int changeHp = Mathf.CeilToInt(maxHp * b.hpBonus);
        if (IsAlive() && changeHp > 0)
        {
            this.hp += changeHp;
            addHp += changeHp;
            EventHandler.CallPlayerHpValChangeEvent(GetPlayerIndex(), GetHpVal());
        }
        if (duration > 0)
        {
            buffToRel.Add(b, StartCoroutine(BuffTimeCountdown(duration, b)));
        }
        return b;
    }
    public void RemoveBuff(string name)
    {
        if (buffs.TryGetValue(name, out Buff b))
        {
            if (buffToRel.TryGetValue(b, out Coroutine c))
            {
                StopCoroutine(c);
                buffToRel.Remove(b);
            }
            int changeHp = Mathf.CeilToInt(maxHp * b.hpBonus) + b.hpNum;
            buffs.Remove(name);
            allBuff.SubBuff(b);

            var curr = GetMaxHP();
            var max = hp - curr;
            if (max > 0)
            {
                if(IsAlive())
                {
                    hp = Mathf.Min(hp - changeHp, 1);
                    EventHandler.CallPlayerHpValChangeEvent(GetPlayerIndex(), GetHpVal());
                }
            }
            EventHandler.CallBuffRemoveEvent(GetPlayerIndex(), b);
        }
    }
    public void ApplyBuff(string name, float duration, float atk, float speed, float atkRange, float atkSpeed, float hp, ApplyBuffType type)
    {
        Buff b = type switch
        {
            ApplyBuffType.NoOverride => ApplyBuffNoOverride(name, duration, atk, speed, atkRange, atkSpeed, hp),
            ApplyBuffType.Override => ApplyBuffOverride(name, duration, atk, speed, atkRange, atkSpeed, hp),
            ApplyBuffType.Add => ApplyBuffAdd(name, duration, atk, speed, atkRange, atkSpeed, hp),
            _ => ApplyBuffNoOverride(name, duration, atk, speed, atkRange, atkSpeed, hp),
        };
        EventHandler.CallBuffChangeEvent(GetPlayerIndex(), b);
    }
    public void ApplyBuff(string name, float duration, Characteristic ch, int val, ApplyBuffType type)
    {
        Buff b = type switch
        {
            ApplyBuffType.NoOverride => ApplyBuffNoOverride(name, duration, ch, val, type),
            ApplyBuffType.Override => ApplyBuffOverride(name, duration, ch, val, type),
            ApplyBuffType.Add => ApplyBuffAdd(name, duration, ch, val, type),
            _ => ApplyBuffNoOverride(name, duration, ch, val, type),
        };
        EventHandler.CallBuffChangeEvent(GetPlayerIndex(), b);
    }
    public void ApplyBuff(string name, float duration, Characteristic ch, float bonus, ApplyBuffType type)
    {
        Buff b = type switch
        {
            ApplyBuffType.NoOverride => ApplyBuffNoOverride(name, duration, ch, bonus, type),
            ApplyBuffType.Override => ApplyBuffOverride(name, duration, ch, bonus, type),
            ApplyBuffType.Add => ApplyBuffAdd(name, duration, ch, bonus, type),
            _ => ApplyBuffNoOverride(name, duration, ch, bonus, type),
        };
        EventHandler.CallBuffChangeEvent(GetPlayerIndex(), b);
    }

    public Buff ApplyBuffNoOverride(string name, float duration, float atk, float speed, float atkRange, float atkSpeed, float hp)
    {
        if (buffs.TryGetValue(name,out Buff b))
        {
            return b;
        }
        return AddBuff(name, duration, atk, speed, atkRange, atkSpeed,hp);
    }
    public Buff ApplyBuffNoOverride(string name, float duration, Characteristic ch, int val, ApplyBuffType type)
    {
        if (buffs.TryGetValue(name, out Buff b))
        {
            return b;
        }
        return AddBuff(name, duration, ch, val, type);
    }
    public Buff ApplyBuffNoOverride(string name, float duration, Characteristic ch, float val, ApplyBuffType type)
    {
        if (buffs.TryGetValue(name, out Buff b))
        {
            return b;
        }
        return AddBuff(name, duration, ch, val, type);
    }
    public Buff ApplyBuffOverride(string name, float duration, float atk, float speed, float atkRange, float atkSpeed, float hp)
    {
        if (buffs.ContainsKey(name))
        {
            RemoveBuff(name);
        }
        return AddBuff(name, duration, atk, speed, atkRange, atkSpeed, hp);
    }
    public Buff ApplyBuffOverride(string name, float duration, Characteristic ch, int val, ApplyBuffType type)
    {
        if (buffs.ContainsKey(name))
        {
            RemoveBuff(name);
        }
        return AddBuff(name, duration, ch, val, type);
    }
    public Buff ApplyBuffOverride(string name, float duration, Characteristic ch, float val, ApplyBuffType type)
    {
        if (buffs.ContainsKey(name))
        {
            RemoveBuff(name);
        }
        return AddBuff(name, duration, ch, val, type);
    }
    public Buff ApplyBuffAdd(string name, float duration, float atk, float speed, float atkRange, float atkSpeed, float hp)
    {
        if (buffs.TryGetValue(name, out Buff b))
        {
            b.AddBuff(atk, speed, atkRange, atkSpeed, hp);
            return b;
        }
        else
        {
            return AddBuff(name, duration, atk, speed, atkRange, atkSpeed, hp);
        }
    }
    public Buff ApplyBuffAdd(string name, float duration, Characteristic ch, int val, ApplyBuffType type)
    {
        if (buffs.TryGetValue(name, out Buff b))
        {
            b.AddBuff(ch,val);
            return b;
        }
        else
        {
            return AddBuff(name, duration, ch, val, type);
        }
    }
    public Buff ApplyBuffAdd(string name, float duration, Characteristic ch, float val, ApplyBuffType type)
    {
        if (buffs.TryGetValue(name, out Buff b))
        {
            b.AddBuff(ch, val);
            return b;
        }
        else
        {
            return AddBuff(name, duration, ch, val, type);
        }
    }
    IEnumerator BuffTimeCountdown(float duration, Buff b)
    {
        yield return new WaitForSeconds(duration);
        buffToRel.Remove(b);
        RemoveBuff(b.buffName);
    }
    public virtual int GetPlayerIndex()
    {
        return -1;
    }
    public bool IsPlayer()
    {
        return GetPlayerIndex() != -1;
    }
    public void RemoveAllBuff()
    {
        buffs.Clear();
        allBuff.Clear();
        //todo show ui
    }
}