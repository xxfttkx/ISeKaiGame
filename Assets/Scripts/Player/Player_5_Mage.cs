using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_5_Mage : Player
{
    public Transform laser;
    public SpriteRenderer laserSp;
    protected override void Awake()
    {
        character.index = 5;
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Attack());
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    IEnumerator Attack()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, this.GetAttackRange());
            if (e != null)
            {
                
                yield return AttackEnemy(e);
                
                yield return null;
            }
            else
            {
                laserSp.size = new Vector2(0, laserSp.size.y);
                yield return null;
            }

        }

    }
    IEnumerator AttackEnemy(EnemyBase e)
    {
        float cd = 10.0f / GetAttackSpeed();
        int atk = GetAttack();
        int count = Mathf.CeilToInt(cd / 0.01f);
        int time = Mathf.CeilToInt(count*1.0f / atk);
        for (int i = 1; i <= count; ++i)
        {
            var dir = e.transform.position - this.transform.position;
            var dis = dir.magnitude;
            if (dis > GetAttackRange()) break;
            laserSp.size = new Vector2(dis, laserSp.size.y);
            // 获取角度（弧度）
            float angle = Mathf.Atan2(dir.y, dir.x);
            // 将角度转换为度数
            float angleInDegrees = angle * Mathf.Rad2Deg;
            // 创建一个新的旋转Quaternion
            Quaternion newRotation = Quaternion.Euler(0f, 0f, angleInDegrees);
            // 将新的旋转应用于Transform
            laser.rotation = newRotation;
            if (i % time == 0)
            {
                PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e, 1);
                if (!e.IsAlive())
                {
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        laserSp.size = new Vector2(0, laserSp.size.y);

    }
    public override int GetAttack()
    {
        return Mathf.CeilToInt(base.GetAttack() * (1.01f - GetHpVal()));
    }
}
