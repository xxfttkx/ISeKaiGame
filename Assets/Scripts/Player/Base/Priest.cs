using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : Player
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Heal());
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    public virtual IEnumerator Heal()
    {
        yield return null;
        while (true)
        {
            HealSkill();
            yield return new WaitForSeconds(10 / character.attackSpeed);
        }

    }
    public virtual void HealSkill()
    {
        foreach (var p in PlayerManager.Instance.players)
        {
            if (!p.IsAlive()) continue;
            PlayerManager.Instance.PlayerHealPlayer(character.index, p.character.index);
        }
    }
}
