using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_23_Warrior : Player_Area
{
    protected override void Awake()
    {
        character.index = 23;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(HurtSelf());
    }
    public override void Reset()
    {
        base.Reset();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override IEnumerator AttackAnim(List<EnemyBase> enemies)
    {
        yield break;
    }
    protected override void AttackEnemies(List<EnemyBase> enemies)
    {
        return;
    }
    public override void BeHurt(int attack)
    {
        if (UnityEngine.Random.Range(0f, 1f)> GetEvasionRate())
            base.BeHurt(attack);
    }
    protected  IEnumerator HurtSelf()
    {
        while(true)
        {
            PlayerManager.Instance.PlayerHurtPlayer(23, 23, GetHurtSelfAtk());
            yield return new WaitForSeconds(GetHurtSelfCD());
        }
    }
    private float GetEvasionRate()
    {
        return 0.5f;
    }
    private float GetHurtSelfCD()
    {
        return 0.5f;
    }
    private int GetHurtSelfAtk()
    {
        return 1;
    }
}
