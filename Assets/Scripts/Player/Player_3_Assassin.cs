using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_3_Assassin : Player
{
    public List<GameObject> knives;
    private HashSet<int> atkingEnemies;
    private List<Coroutine> atks;
    protected override void Awake()
    {
        character.index = 3 ;
        base.Awake();
        atkingEnemies = new HashSet<int>();
        atks = new List<Coroutine>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.PlayerKillEnemyEvent += OnPlayerKillEnemyEvent;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.PlayerKillEnemyEvent -= OnPlayerKillEnemyEvent;
    }
    protected override void OnEnterLevelEvent(int l)
    {
        base.OnEnterLevelEvent(l);
    }
    public override void Reset()
    {
        base.Reset();
        atkingEnemies.Clear();

        foreach (var go in knives)
        {
            go.SetActive(false);
        }
        
    }
    public override void StartAttack()
    {
        base.StartAttack();
        AttackByExtra(0);
        AttackByExtra(1);
    }
    private void OnPlayerKillEnemyEvent(int playerIndex)
    {
        if (3 == playerIndex)
        {
            int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 2);
            if (extra == 0) PlayerManager.Instance.PlayerHurtPlayer(GetPlayerIndex(), GetPlayerIndex(), Mathf.FloorToInt(hp / 2f));
            if (extra == 1) PlayerManager.Instance.PlayerHurtPlayer(GetPlayerIndex(), GetPlayerIndex(), hp-1);
            if (extra == 2) return;
        }
    }
        
    IEnumerator Attack(int index)
    {
        if(index==0)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, GetAttackRange());
            if (e != null)
            {
                yield return AttackAnim(e, knives[index]);
                yield return WaitCDAndJudgeExtra(index);
                yield break;
            }
        }
        yield return null;
        AttackByExtra(index);
    }
    IEnumerator AttackDesire1(int index)
    {
        var enemies = Utils.GetNearEnemiesSortByDistance(this.transform.position, GetAttackRange());
        if (enemies != null && enemies.Count > 0)
        {
            foreach (var e in enemies)
            {
                if ((e.transform.position.y - this.transform.position.y) * (0.5f - index) >= 0)
                {
                    yield return AttackAnim(e, knives[index]);
                    break;
                }
            }
            yield return WaitCDAndJudgeExtra(index);
            yield break;
        }
        yield return null;
        AttackByExtra(index);
    }
    IEnumerator AttackDesire2(int index)
    {
        var enemies = Utils.GetNearEnemiesSortByDistance(this.transform.position, GetAttackRange());
        if (enemies != null && enemies.Count > 0)
        {
            int a = PlayerManager.Instance.GetPlayerAttack(3);
            foreach (var e in enemies)
            {
                if (atkingEnemies.Contains(e.GetGlobalIndex())) continue;
                if (a < e.GetHP())
                {
                    atkingEnemies.Contains(e.GetGlobalIndex());
                    yield return AttackAnim(e, knives[index]);
                    atkingEnemies.Remove(e.GetGlobalIndex());
                    break;
                }
            }
            yield return WaitCDAndJudgeExtra(index);
            yield break;
        }
        yield return null;
        AttackByExtra(index);
    }
    IEnumerator AttackAnim(EnemyBase e, GameObject knife)
    {
        // 0.1f 5
        Vector2 dir = e.transform.position - this.transform.position;
        Vector2 bas = dir.normalized;
        Vector2 curr = Vector2.zero;
        float angle = Vector2.Angle(Vector2.up, dir);
        if (dir.x > 0)
        {
            angle = -angle;
        }
        knife.transform.rotation = Quaternion.Euler(0, 0, angle);
        knife.SetActive(true);
        while(curr.sqrMagnitude<dir.sqrMagnitude)
        {
            curr += bas / 2f;
            knife.transform.localPosition = curr;
            yield return new WaitForSeconds(0.01f);
        }
        PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e);
        while (Vector2.Dot(curr, bas) > 0)
        {
            curr -= bas / 2f;
            knife.transform.localPosition = curr;
            yield return new WaitForSeconds(0.01f);
        }
        knife.SetActive(false);
        
    }
    public override void AddBuffBeforeStart()
    {
        float b = GetBuffBonus();
        ApplyBuff("player3", -1, 0f, b, 0f, 0f, 0f, ApplyBuffType.Override);
    }
    private float GetBuffBonus()
    {
        int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 2);
        if (extra == 0) return 0.5f;
        if (extra == 1) return 1.0f;
        if (extra == 2) return 0.1f;
        return 0f;
    }
    protected override void OnDesireChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        base.OnDesireChangeEvent(playerIndex, extraIndex, selectedIndex);
        if (playerIndex == GetPlayerIndex() && extraIndex == 2)
        {
            AddBuffBeforeStart();
        }
    }
    IEnumerator WaitCDAndJudgeExtra(int index)
    {
        yield return new WaitForSeconds(GetSkillCD());
        AttackByExtra(index);
    }
    void AttackByExtra(int index)
    {
        int desire = extras[1];
        if (desire == 0)
        {
            StartCoroutine(Attack(index));
        }
        else if (desire == 1)
        {
            StartCoroutine(AttackDesire1(index));
        }
        else if (desire == 2)
        {
            StartCoroutine(AttackDesire2(index));
        }
    }
}
