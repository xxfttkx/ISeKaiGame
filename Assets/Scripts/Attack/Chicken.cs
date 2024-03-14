using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    private float searchDelta = 0.02f;
    Player player;
    List<int> extras;
    int atk;
    int range;
    int atkSpeed;
    protected void OnEnable()
    {
        player = PlayerManager.Instance.indexToPlayer[6];
        //extras = new List<int>(SaveLoadManager.Instance.GetPlayerExtras(6));
        extras = PlayerManager.Instance.GetPlayerExtras(6);
        atk = PlayerManager.Instance.GetPlayerAttack(6);
        range = PlayerManager.Instance.GetPlayerAttackRange(6);
        atkSpeed = PlayerManager.Instance.GetPlayerAttack(6);
        if (extras[1]==1)
        {
            StartCoroutine(Heal());
        }
        StartCoroutine(SearchEnemy());
        StartCoroutine(Attack());
        StartCoroutine(AutoRelease());
    }
    IEnumerator SearchEnemy()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, player.GetAttackRange());
            if(e!=null)
            {
                Vector2 dir = e.transform.position - this.transform.position;
                Vector2 dis = dir.normalized * searchDelta * player.GetSpeed();
                this.transform.position = this.transform.position + (Vector3)dis;
                yield return new WaitForSeconds(searchDelta);
            }
            else
            {
                yield return null;
            }
            
        }

    }
    IEnumerator Attack()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, 0.6f);
            if(e!=null)
            {
                PlayerManager.Instance.PlayerHurtEnemy(6, e);
            }
            else
            {
                yield return null;
            }
        }
    }
    IEnumerator Heal()
    {
        while (true)
        {
            var p = PlayerManager.Instance.GetPlayerInControl();
            if (Utils.TryAttackPlayer(this.gameObject, player, range))
            {
                PlayerManager.Instance.PlayerHealPlayer(6, p.GetPlayerIndex(), atk);
            }
        }
    }
    IEnumerator AutoRelease()
    {
        yield return new WaitForSeconds(10f);
        PoolManager.Instance.ReleaseObj(this.gameObject, 7);
    }
}
