using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPShow : MonoBehaviour
{
    public RectTransform hpRect;
    public int minWidth = 16;
    public int maxWidth = 100;

    private void OnEnable()
    {
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
    }
    private void OnDisable()
    {
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
    }
    void OnExitLevelEvent(int _)
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }
    public void Init(Creature c,float yOffset)
    {
        StartCoroutine(FollowCreature(c, yOffset));
    }
    public void SetHpVal(float val)
    {
        var width = Mathf.Lerp(minWidth, maxWidth, val);
        hpRect.sizeDelta = new Vector2(width, hpRect.sizeDelta.y);
    }
    IEnumerator FollowCreature(Creature c, float yOffset)
    {
        var t = this.transform;
        var offset = new Vector3(0, yOffset, 0);
        while (true)
        {
            if (c.IsAlive())
            {
                t.position = Camera.main.WorldToScreenPoint(c._pos) + offset;
                yield return null;
            }
            else
            {

                break;
            }
        }
        Destroy(this.gameObject);
    }
    
}
