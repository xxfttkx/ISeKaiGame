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


//TODO:�������Ͷ���
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

//ְҵ
public enum Profession
{ 
    Warrior = 0,    //սʿ
    Priest,     //��ʦ
    Mage,       //ħ��ʦ
    Assassin,   //�̿�
    Max,
}


public enum EnemyType
{
    MeleeEnemy = 1, // ��ս����
    RangedEnemy = 2, // Զ�̵���
    Boss = 3,
}

public enum Language
{
    Chinese = 0,
    English = 1,
    Japanese = 2,

}