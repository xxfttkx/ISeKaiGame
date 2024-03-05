using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_0_Warrior : Player_Area
{
    public GameObject circle;
    protected override void Awake()
    {
        character.index = 0;
        base.Awake();
        circle.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override IEnumerator AttackAnim(List<EnemyBase> enemies)
    {
        AttackEnemies(enemies);
        int range = GetAttackRange();
        circle.transform.localScale = new Vector3(range, range, 1);
        circle.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        circle.SetActive(false);
    }

    public override int GetAttack()
    {
        int atk = base.GetAttack();
        atk = Mathf.CeilToInt(atk * (1.5f));
        return atk;
    }

    public override float GetMoneyEfficiency()
    {
        return 0.2f;
    }
}
