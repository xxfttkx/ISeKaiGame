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
{                    //        hp   atk    speed    atkSpeed    atkRange                          
    Warrior = 0,    //սʿ     50   8       4          3          2
    Priest,     //��ʦ         20   10      6          5          5
    Mage,       //ħ��ʦ       10   10      5          5          6
    Assassin,   //�̿�         8    15      6          10         3
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
    NoOverride, //������
    Override, //����
    Add, //��� ʱ�乲�� .. ̫�鷳�� ��Ĭ�Ϲ���Ķ���Buffʱ�����õ�
}