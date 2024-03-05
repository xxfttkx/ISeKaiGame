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
{ 
    Warrior = 0,    //战士
    Priest,     //牧师
    Mage,       //魔法师
    Assassin,   //刺客
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

}