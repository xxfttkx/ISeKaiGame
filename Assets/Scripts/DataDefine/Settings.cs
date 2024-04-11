using System.Collections.Generic;
using UnityEngine;
public class Settings
{
    //人物收到伤害后，有0.2s的无敌保护
    public const float beHurtInvincibleTime = 0.2f;
    //切人冷却3s
    public const float switchPlayerCd = 3f;

    public const float guangQiuExistTime = 7f;
    //最多关卡数
    public const int levelMaxNum = 10;
    //预留敌人数
    public const int reservedEnemyCount = 500;
    // 判断击中玩家距离
    public const float hitPlayerDis = 0.7f;
    public const float hitPlayerDisSqr = 0.49f;
    public const float hitEnemyDis = 0.7f;
    /*    public Dictionary<int, KeyCode> changePlayerKey = new Dictionary<int, KeyCode>() {1=KeyCode.Alpha1 };*/
    public const float circleAnimTime = 0.3f;
    public const float jumpDuration = 0.3f;
    public const float basePanelShowTime = 0.3f;
    public const float hurtEnemyShowHpTime = 0.3f;
    public const float minusHPTime = .1f;
    public const int maxCompanionNum = 10;
}