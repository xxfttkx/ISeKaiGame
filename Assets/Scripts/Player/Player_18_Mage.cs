using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_18_Mage : Player
{
    private float lastChantTime;
    public List<Sprite> chantList;
    public SpriteRenderer chantSp;
    protected override void Awake()
    {
        character.index = 18;
        chantSp.enabled = false;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Attack());
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    IEnumerator Attack()
    {
        while (true)
        {
            bool attackSuccess = false;
            do
            {
                var e = Utils.GetNearestEnemy(this.transform.position, this.GetAttackRange());
                if (e != null)
                {
                    int lastGlobalIndex = e.GetGlobalIndex();
                    yield return Chant();
                    int newGlobalIndex = e.GetGlobalIndex();
                    if (lastGlobalIndex != newGlobalIndex)
                    {
                        Debug.Log("target was dead¡£¡£¡£");
                        break;
                    }
                    PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e);
                    AudioManager.Instance.PlaySoundEffect(SoundName.Atk);
                    attackSuccess = true;
                }
            } while (false);
            if(!attackSuccess)
            {
                yield return null;
            }
            else
            {
                var cd = 10.0f / GetAttackSpeed();
                yield return new WaitForSeconds(cd);
            }
        }

    }

    IEnumerator Chant()
    {
        lastChantTime = GetChantTime();
        chantSp.enabled = true;
        float temp = lastChantTime - 0.1f;
        float delta = temp / chantList.Count;
        int index = 0;
        for (float curr = 0;curr< temp; curr+= delta)
        {
            chantSp.sprite = chantList[index];
            index++;
            yield return new WaitForSeconds(delta);
        }
        yield return new WaitForSeconds(0.1f);
        chantSp.enabled = false;

    }

    private float GetChantTime()
    {
        return 1.0f;
    }
    public override int GetAttack()
    {
        int a = base.GetAttack();
        a = Mathf.CeilToInt(a * lastChantTime);
        return a;
    }
}
