public enum GameState
{
    GamePlay,
    GamePause,
    GameEnd,
    ExitLevel,
}

public enum FireType
{
    Single,
    Triple
}

public enum SoundName
{
    MeeleAtk,
    Projectile,
    EnemyProjectile,
    HurtEnemy,
    BeHurt,
    Atk,
    Button,
}
public enum BGMName
{
    Fantasy,
    Battle
}


public enum ExtraType
{
    Hurt,
    BeHurt,
    Heal,
    Kill,
    EnterLevel,
    ExitLevel,
    EnterNum,
    ExitNum,
    Max,
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
    Hp,
    Attack,
    Speed,
    AttackSpeed,
    AttackRange,
    Max,
}
public enum CharacteristicBonus
{
    ProjectileSpeedBonus,
}

public enum ApplyBuffType
{
    None = 0,
    NoOverride, //������
    Override, //����
    Add, //��� ʱ�乲�� .. ̫�鷳�� ��Ĭ�Ϲ���Ķ���Buffʱ�����õ�
}