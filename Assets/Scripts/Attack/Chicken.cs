using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    private float searchDelta = 0.02f;
    Player player;
    protected void OnEnable()
    {
        player = PlayerManager.Instance.indexToPlayer[6];
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
    IEnumerator AutoRelease()
    {
        yield return new WaitForSeconds(10f);
        PoolManager.Instance.ReleaseObj(this.gameObject, 7);
    }
}
