using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Area : Player
{
    protected override void Awake()
    {
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
    protected virtual IEnumerator Attack()
    {
        while (true)
        {
            var enemies = Utils.GetNearEnemies(this.transform.position, this.GetAttackRange());
            if (enemies != null && enemies.Count > 0)
            {
                yield return StartCoroutine(AttackAnim(enemies));
                yield return GetSkillCD();
            }
            else
            {
                yield return null;
            }
        }

    }
    protected virtual IEnumerator AttackAnim(List<EnemyBase> enemies)
    {
        yield break;
    }
    protected virtual void AttackEnemies(List<EnemyBase> enemies)
    {
        foreach (var e in enemies)
        {
            PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e);
        }
    }
}
