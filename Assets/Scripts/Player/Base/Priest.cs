using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : Player
{ 
    public override void StartAttack()
    {
        StartCoroutine(Heal());
    }
    public virtual IEnumerator Heal()
    {
        yield return null;
        while (true)
        {
            HealSkill();
            yield return new WaitForSeconds(GetSkillCD());
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
