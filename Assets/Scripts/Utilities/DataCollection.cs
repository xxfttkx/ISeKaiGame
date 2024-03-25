using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct CreatureBase
{
    [Header("Attributes")]
    public int attack;
    public int hp;
    public int speed;
    public int attackSpeed; // 10s内攻击次数
    public int attackRange; // 攻击距离
    public bool faceToLeft;
    public Sprite sprite;
    public GameObject prefab;
    [TextArea(3, 10)]
    public string desc;
}

[System.Serializable]
public struct Character
{
    public int index;
    public CreatureBase creature;
    public string name;
    public Profession profession;
    [Header("Attributes")]
/*    public int attack;
    public int hp;
    public int speed;
    public int attackSpeed; // 10s内攻击次数
    public int attackRange; // 攻击距离*/
    [TextArea(3, 10)]
    public string desc;
    public List<ExtraType> extraTypes;
    public List<int> extraThresholds;
    public List<string> extraDesire1;
    public List<string> extraDesire2;
    public Characteristic[] extraCharacteristics;
    public int[] extraCharacteristicVals;
    public int[] extraData;
    public bool finished;
}

[System.Serializable]
public struct Enemy
{
    public CreatureBase creature;
    public int index;
    public string name;
    public Sprite sprite;
    public GameObject prefab;
    public bool faceToLeft;
    public EnemyType enemyType;
    [Header("Attributes")]
    public int attack;
    public int hp;
    public int speed;
    public float getPlayerPosTimeDelta;
    public float attackRange;
    public float attackSpeed;
    public int exp;
    public int money;
    [TextArea(3, 10)]
    public string desc;
}

[System.Serializable]
public struct LevelCreatEnemy
{
    public int levelIndex;
    public float bonus;
    public Vector2[] offset;
    public int[] enemyIndex;
    public int[] enemyCreateFirstTime;
    public int[] enemyCreateDeltaTime;
    // 从第几秒后不出怪
    public int endCreatEnemyTime;

}
[System.Serializable]
public class SoundDetails
{
    public SoundName soundName;
    public AudioClip soundClip;
    [Range(0.1f, 1.5f)]
    public float soundPitchMin;
    [Range(0.1f, 1.5f)]
    public float soundPitchMax;
    [Range(0.1f, 1f)]
    public float soundVolume;
}
[System.Serializable]
public class GenerateEnemyPointData
{
    public int[] enemyIndex;
    public int[] enemyCreateFirstTime;
    public int[] enemyCreateDeltaTime;

}

[System.Serializable]
public class Buff
{
    public string buffName;
    public float duration;
    public float attackBonus;
    public float speedBonus;
    public float attackRangeBonus;
    public float attackSpeedBonus;
    public float hpBonus;
    public int hpNum;
    public int atkNum;
    public int speedNum;
    public int atkSpeedNum;
    public int atkRangeNum;
    public float ProjectileSpeedBonus;
    public Buff(string name, Characteristic characteristic, int val)
    {
        this.buffName = name;
        _ = characteristic switch
        {
            Characteristic.Hp => hpNum = val,
            Characteristic.Attack => atkNum = val,
            Characteristic.Speed => speedNum = val,
            Characteristic.AttackSpeed => atkSpeedNum = val,
            Characteristic.AttackRange => atkRangeNum = val,
            
            _ => val,
        };
    }
    public Buff(string name, Characteristic characteristic, float val)
    {
        this.buffName = name;
        _ = characteristic switch
        {
            Characteristic.ProjectileSpeedBonus => ProjectileSpeedBonus = val,
            _ => val,
        };
    }
    public Buff(string name, float atk, float speed, float atkRange, float atkSpeed)
    {
        this.buffName = name;
        this.attackBonus = atk;
        this.speedBonus = speed;
        this.attackRangeBonus = atkRange;
        this.attackSpeedBonus = atkSpeed;
    }

    public Buff(string name)
    {
        buffName = name;
    }

    public void AddBuff(Buff b)
    {
        this.attackBonus += b.attackBonus;
        this.speedBonus += b.speedBonus;
        this.attackRangeBonus += b.attackRangeBonus;
        this.attackSpeedBonus += b.attackSpeedBonus;
        this.hpBonus += b.hpBonus;
        this.hpNum += b.hpNum;
        this.atkNum += b.atkNum;
        this.speedNum += b.speedNum;
        this.atkSpeedNum += b.atkSpeedNum;
        this.atkRangeNum += b.atkRangeNum;
        this.ProjectileSpeedBonus += b.ProjectileSpeedBonus;
    }
    public void SubBuff(Buff b)
    {
        this.attackBonus -= b.attackBonus;
        this.speedBonus -= b.speedBonus;
        this.attackRangeBonus -= b.attackRangeBonus;
        this.attackSpeedBonus -= b.attackSpeedBonus;
        this.hpBonus -= b.hpBonus;
        this.hpNum -= b.hpNum;
        this.atkNum -= b.atkNum;
        this.speedNum -= b.speedNum;
        this.atkSpeedNum -= b.atkSpeedNum;
        this.atkRangeNum -= b.atkRangeNum;
        this.ProjectileSpeedBonus -= b.ProjectileSpeedBonus;
    }
    public void Clear()
    {
        this.attackBonus = 0f;
        this.speedBonus = 0f;
        this.attackRangeBonus = 0f;
        this.attackSpeedBonus = 0f;
        this.hpBonus = 0f;
        this.hpNum = 0;
        this.atkNum = 0;
        this.speedNum = 0;
        this.atkSpeedNum = 0;
        this.atkRangeNum = 0;
        this.ProjectileSpeedBonus = 0;
    }

    public void AddBuff(float atk, float speed, float atkRange, float atkSpeed, float hp)
    {
        this.attackBonus += atk;
        this.speedBonus += speed;
        this.attackRangeBonus += atkRange;
        this.attackSpeedBonus += atkSpeed;
        this.hpBonus += hp;
    }
    public void AddBuff(Characteristic characteristic, int val)
    {
        _ = characteristic switch
        {
            Characteristic.Hp => hpNum += val,
            Characteristic.Attack => atkNum += val,
            Characteristic.Speed => speedNum += val,
            Characteristic.AttackSpeed => atkSpeedNum += val,
            Characteristic.AttackRange => atkRangeNum += val,
            Characteristic.ProjectileSpeedBonus => ProjectileSpeedBonus += val,
            _ => val,
        };
    }
    public void AddBuff(Characteristic characteristic, float val)
    {
        _ = characteristic switch
        {
            Characteristic.ProjectileSpeedBonus => ProjectileSpeedBonus += val,
            _ => val,
        };
    }
}

[System.Serializable]
public class LanguageToText
{
    public List<string> texts;
}

[System.Serializable]
public struct ProfessionData
{
    public Profession profession;
    public int hp;
    public int attack;
    public int speed;
    public int attackSpeed; // 10s内攻击次数
    public int attackRange;
}

