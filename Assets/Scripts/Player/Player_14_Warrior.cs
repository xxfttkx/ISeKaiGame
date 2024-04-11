using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_14_Warrior : Player_Area
{
    int addHpLimit;
    public SpriteRenderer circle;
    protected override void Awake()
    {
        character.index = 14;
        base.Awake();
    }
    public override void Reset()
    {
        base.Reset();
        addHpLimit = 50;
        circle.enabled = false;
    }
    public override void StartAttack()
    {
        base.StartAttack();
        StartCoroutine(DoExtra1());
    }
    protected override IEnumerator AttackAnim(List<EnemyBase> enemies)
    {
        while(true)
        {
            if(extras[2]==2)
            {
                //TODO Anim
                StartCoroutine(AtkAnim());
                AttackEnemies(enemies);
            }
            else
            {
                yield return null;
            }
        }
    }
    IEnumerator AtkAnim()
    {
        circle.size = new Vector2(_range, _range);
        circle.enabled = true;
        yield return new WaitForSeconds(Settings.circleAnimTime);
        circle.enabled = false;
    }
    IEnumerator DoExtra1()
    {
        while(true)
        {
            if(extras[1]==1)
            {
                PlayerManager.Instance.PlayerHealPlayer(GetPlayerIndex(), GetPlayerIndex(), 1);
                yield return new WaitForSeconds(1f);
            }
            else if (extras[1] == 2)
            {
                if(addHpLimit>0)
                {
                    --addHpLimit;
                    AddHpLimit(1);
                    yield return new WaitForSeconds(2f);
                }
            }
            yield return null;
        }
    }
}
