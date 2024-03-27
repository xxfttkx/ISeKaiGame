using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_15_Warrior : Player_Area
{
    public SpriteRenderer circle;
    public SpriteRenderer outlineSp;
    public Material outline;
    private int count;
    private int maxAddHpCount;
    private Coroutine showOutline;
    protected override void Awake()
    {
        character.index = 15;
        outline = outlineSp.material;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.PlayerHurtPlayerEvent += OnPlayerHurtPlayerEvent;
    }
    public override void Reset()
    {
        base.Reset();
        count = 0;
        maxAddHpCount = 50;
        outline.SetFloat("_Outline", 0);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.PlayerHurtPlayerEvent -= OnPlayerHurtPlayerEvent;
    }
    protected override IEnumerator AttackAnim(List<EnemyBase> enemies)
    {
        StartCoroutine(AtkAnim());
        AttackEnemies(enemies);
        yield break;
    }
    IEnumerator AtkAnim()
    {
        circle.size = new Vector2(_range, _range);
        circle.enabled = true;
        yield return new WaitForSeconds(Settings.circleAnimTime);
        circle.enabled = false;
    }

    float _bonus
    {
        get => extras[2] switch
        {
            0 => -0.9f,
            1 => -0.99f,
            2 => 0.5f,
            _ => 0f,
        };
    }
    public override void AddBuffBeforeStart()
    {
        ApplyBuff("player15", -1, 0f, _bonus, 0f, 0f, 0f, ApplyBuffType.Override);
    }
    protected override void OnDesireChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        base.OnDesireChangeEvent(playerIndex, extraIndex, selectedIndex);
        if (playerIndex == GetPlayerIndex() && extraIndex == 2)
        {
            AddBuffBeforeStart();
        }
    }

    int _needHitNum
    {
        get => extras[2] switch
        {
            0 => 4,
            1 => 2,
            _ => int.MaxValue,
        };
    }
    public override void BeHurt(int attack,EnemyBase e)
    {
        if(extras[1]==2)
        {
            if(maxAddHpCount>0)
            {
                --maxAddHpCount;
                addHp++;
                EventHandler.CallPlayerHpValChangeEvent(GetPlayerIndex(), GetHpVal());
            }
        }
        if (count >= _needHitNum)
        {
            if (showOutline != null) StopCoroutine(showOutline);
            StartCoroutine(HideOutline());
            return;
        }
        base.BeHurt(attack, e);
        count++;
        if (count >= _needHitNum)
        {
            showOutline = StartCoroutine(ShowOutline());
        }
    }
    void OnPlayerHurtPlayerEvent(int atkIndex, int hurtIndex, int atk)
    {
        if (hurtIndex == GetPlayerIndex() && extras[1] == 1)
        {
            StartCoroutine(DelayHeal(atk));
        }
    }
    IEnumerator DelayHeal(int num)
    {
        yield return new WaitForSeconds(1f);
        PlayerManager.Instance.PlayerHealPlayer(GetPlayerIndex(), GetPlayerIndex(), num);                                                                                              
    }
    float duration = 0.3f;
    IEnumerator ShowOutline()
    {
        float t = 0;
        while (true)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0f, 2f, t / duration);
            outline.SetFloat("_Outline", a);
            if (t > duration) yield break;
            yield return null;
        }
    }
    IEnumerator HideOutline()
    {
        float t = 0;
        while (true)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(2f, 0f, t / duration);
            outline.SetFloat("_Outline", a);
            if (t > duration) yield break;
            yield return null;
        }
    }
}
