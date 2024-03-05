using UnityEngine;
public class Settings
{
    public const int bulletRepelForce = 100;
    public const float maxRandomShifting = 100f;
    public const int bulletSpeed = 1000;
    public const int bulletDamage = 10;
    public const float bulletExistTime = 3f;
    public const float hitHighLightTime = 0.5f;
    public const int playerHealth = 5;
    public const float TripleShootShifting = 100f;

    public const float playerWidth = 1.2f;

    //开枪减速时间
    public const float shootSlowTime = 0.5f;
    //开枪减速时速度
    public const int shootSlowSpeed = 4;
    //爆炸范围
    public const float explodeRange = 2f;
    //
    public const float fadeDuration = 1f;
    public const float dialogueTextShowTime = 1f;
    public static Vector3 playerStartPos = new Vector3(0, 0, 0);


    //5s没有收到伤害后，恢复1hp，之后每3s恢复1hp
    public const float addHPTime = 1f;

    //人物收到伤害后，有0.2s的无敌保护
    public const float beHurtInvincibleTime = 0.2f;
    //玩家血量
    public const float playerHP = 10f;
    //熊击退
    public const float bearRepelForce = 100f;
    //切人冷却3s
    public const float switchPlayerCd = 3f;

    public const float guangQiuExistTime = 7f;

    //队伍中最多人数
    public const int playerMaxNum = 3;

    //最多关卡数
    public const int levelMaxNum = 50;
    //预留敌人数
    public const int reservedEnemyCount = 500;
    // 判断击中玩家距离
    public const float hitPlayerDis = 0.7f;
}