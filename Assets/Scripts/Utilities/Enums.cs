public enum GameState
{
    GamePlay,
    GamePause,
    GameEnd
}

public enum FireType
{
    Single,
    Triple
}

public enum SoundName
{
    Atk = 1,
}


//TODO:武器类型定义
public enum ExtraType
{
    None = 0,
    Hurt,
    BeHurt,
    Heal,
    Kill,
    EnterLevel,
    ExitLevel,
    EnterNum,
    ExitNum,
    Else,
}

//职业
public enum Profession
{                    //        hp   atk    speed    atkSpeed    atkRange                          
    Warrior = 0,    //战士     50   8       4          3          2
    Priest,     //牧师         20   10      6          5          5
    Mage,       //魔法师       10   10      5          5          6
    Assassin,   //刺客         8    15      6          10         3
    Max,
}


public enum EnemyType
{
    MeleeEnemy = 1, // 近战敌人
    RangedEnemy = 2, // 远程敌人
    Boss = 3,
}

public enum Language
{
    Chinese = 0,
    English = 1,
    Japanese = 2,
    Max,

}

public enum Characteristic
{
    None = 0,
    Hp,
    Attack,
    Speed,
    AttackSpeed,
    AttackRange,
}

public enum ApplyBuffType
{
    None = 0,
    NoOverride, //不覆盖
    Override, //覆盖
    Add, //添加 时间共享 .. 太麻烦了 先默认共享的都是Buff时间永久的
}