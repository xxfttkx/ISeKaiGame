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
    int speed;
    float cd;
    bool bReleased;
    Vector2 _pos
    {
        get => this.transform.position;
    }

    /// <summary>
    /// Init
    /// </summary>
    public void Init()
    {
        player = PlayerManager.Instance.indexToPlayer[6];
        extras = PlayerManager.Instance.GetPlayerExtras(6);
        atk = PlayerManager.Instance.GetPlayerAttack(6);
        range = PlayerManager.Instance.GetPlayerAttackRange(6);
        speed = PlayerManager.Instance.GetPlayerSpeed(6);
        atkSpeed = PlayerManager.Instance.GetPlayerAttackSpeed(6);
        cd = 10f / atkSpeed;
        if (extras[1] == 0)
        {
            StartCoroutine(Attack());
            StartCoroutine(SearchEnemy());
        }
        else if (extras[1] == 1)
        {
            StartCoroutine(Heal());
        }
        else
        {
            StartCoroutine(RangedAttack());
        }

        StartCoroutine(AutoRelease());
    }
    protected void OnEnable()
    {
        bReleased = false;
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
    }
    private void OnDisable()
    {
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
    }
    IEnumerator SearchEnemy()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, player.GetAttackRange());
            if(e!=null)
            {
                Vector2 dir = e.transform.position - this.transform.position;
                Vector2 dis = dir.normalized * searchDelta * speed;
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
                yield return new WaitForSeconds(cd);
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
                PoolManager.Instance.CreateEgg(p, null, _pos,atk);
                yield return new WaitForSeconds(cd);
            }
            else
            {
                yield return null;
            }
        }
    }
    IEnumerator RangedAttack()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, range);
            if (e != null)
            {
                PoolManager.Instance.CreateEgg(null, e, _pos, atk);
                yield return new WaitForSeconds(cd);
            }
            else
            {
                yield return null;
            }
        }
    }
    IEnumerator AutoRelease()
    {
        float time = 10f;
        if (extras[3] == 0)time = 10f;
        else if (extras[3] == 1)time = 20f;
        else if (extras[3] == 2)time = 8f;
        yield return new WaitForSeconds(time);
        Release();
    }
    void OnExitLevelEvent(int _)
    {
        Release();
    }
    void Release()
    {
        if (bReleased) return;
        bReleased = true;
        PoolManager.Instance.ReleaseObj(this.gameObject, 7);
    }
}
