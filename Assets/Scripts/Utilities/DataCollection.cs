using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct CreatureBase
{
    public bool faceToLeft;
    [Header("Attributes")]
    public int attack;
    public int hp;
    public int speed;
    public int attackSpeed; // 10s内攻击次数
    public int attackRange; // 攻击距离
    [TextArea(3, 10)]
    public string desc;
}

[System.Serializable]
public struct Character
{
    public CreatureBase creature;
    public int index;
    public string name;
    public Sprite sprite;
    public GameObject prefab;
    public bool faceToLeft;

    public Profession profession;
    [Header("Attributes")]
    public int attack;
    public int hp;
    public int speed;
    public int attackSpeed; // 10s内攻击次数
    public int attackRange; // 攻击距离
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
    // 其他可能的影响属性
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
    }
    public void AddBuff(float bonus)
    {
        this.attackBonus += bonus;
        this.speedBonus += bonus;
        this.attackRangeBonus += bonus;
        this.attackSpeedBonus += bonus;
    }
    public void SubBuff(Buff b)
    {
        this.attackBonus -= b.attackBonus;
        this.speedBonus -= b.speedBonus;
        this.attackRangeBonus -= b.attackRangeBonus;
        this.attackSpeedBonus -= b.attackSpeedBonus;
    }
    public void Clear()
    {
        this.attackBonus = 0f;
        this.speedBonus = 0f;
        this.attackRangeBonus = 0f;
        this.attackSpeedBonus = 0f;
    }
}

[System.Serializable]
public class LanguageToText
{
    public List<string> texts;
}

