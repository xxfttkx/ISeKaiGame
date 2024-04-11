using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy_7_Melee : Enemy_Melee
{
    public List<GameObject> tentacles;
    List<bool> inAtkAnims = new List<bool>();
    protected override bool CanMove
    {
        get
        {
            if (!IsAlive() || isBegingRepelled) return false;
            foreach(var b in inAtkAnims)
            {
                if (b) return false;
            }
            return true;
        }
    }
    protected override void Awake()
    {
        enemy.index = 7;
        base.Awake();
    }

    public override void Reset()
    {
        base.Reset();
        inAtkAnims.Clear();
        foreach (var t in tentacles)
        {
            var sp = t.GetComponent<SpriteRenderer>();
            sp.size = new Vector2(base.sp.size.x, 0);
            inAtkAnims.Add(false);
        }
        for (int i = 0; i < tentacles.Count; ++i)
        {
            StartCoroutine(Attack(tentacles[i],i));
        }
    }
    protected override IEnumerator Attack()
    {
        yield break;
    }
    IEnumerator Attack(GameObject tentacle, int index)
    {
        while (true)
        {
            if (!IsAlive()) break;
            if (isBegingRepelled || player == null)
            {
                yield return null;
                continue;
            }
            
            if (Utils.CanAttackPlayer(tentacle, player, GetAttackRange()))
            {
                inAtkAnims[index] = true;
                //ATK
                var dir = player.transform.position - tentacle.transform.position;
                var angle = Vector2.Angle(Vector2.up, dir);
                if (dir.x > 0) angle = -angle;
                tentacle.transform.rotation = Quaternion.Euler(0, 0, angle);
                yield return Stretch(tentacle, dir);
                inAtkAnims[index] = false;
                yield return new WaitForSeconds(GetSkillCD());
            }
            else
            {
                yield return null;
            }

        }
    }

    IEnumerator Stretch(GameObject tentacle, Vector2 dir)
    {
        // 0.1f 1->2
        var sp = tentacle.GetComponent<SpriteRenderer>();
        float r = GetAttackRange();
        Vector2 diff;
        dir = dir.normalized;
        float minDis = float.MaxValue;
        for (float height = 0; height < r; height += 0.5f)
        {
            sp.size = new Vector2(sp.size.x, height);
            Vector2 ten = dir * height;
            diff = (Vector2)player.transform.position - ((Vector2)tentacle.transform.position + ten);
            if (minDis > diff.sqrMagnitude)
            {
                minDis = diff.sqrMagnitude;
                if (minDis < Settings.hitPlayerDisSqr)
                {
                    PlayerManager.Instance.EnemyHurtPlayer(this, player);
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (float height = sp.size.y; height > 0; height -= 0.5f)
        {
            yield return new WaitForSeconds(0.01f);
            sp.size = new Vector2(sp.size.x, height);
        }
        sp.size = new Vector2(sp.size.x, 0);
    }

}
