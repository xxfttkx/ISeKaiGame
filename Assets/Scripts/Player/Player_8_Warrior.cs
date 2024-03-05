using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_8_Warrior : Player_Area
{
    public SpriteRenderer water;
    protected override void Awake()
    {
        character.index = 8;
        base.Awake();
        water.enabled = false;
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
        int r = GetAttackRange();
        r *= 2;
        water.size = new Vector2(r, r);
        water.enabled = true;
        foreach(var e in enemies)
        {
            PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e, e.GetHP() - 1);
        }
        StartCoroutine(Delay(0.5f));
        yield break;
    }
    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        water.enabled = false;
    }
}
